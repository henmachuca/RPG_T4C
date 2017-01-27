using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//ok this is the click the move and the other opened is the player level system
public class Player_Interaction : MonoBehaviour
{
    //public Player_Stats stats;
    public Player_Attack attackScript;
    public float attackDistance = 0.05f;
    public float attackRate = 0.5f;
    public Animator m_Animator;  //TEMPORARY

    public GameObject target;   // going to create another target just for projectiles - i'll let you go through combining
    public Transform targetTransf;

    public GameObject arrowTest;

    public bool meleeAttacking = false;  //flag to make possible for the player to auto attack
    public bool moveWithTarget;  //allows the player to move while having a target selected without deselecting it

    public Transform feetPos;

    public float doubleClickTimer = 1f;
    public bool didDoubleClick;

    float clicked = 0;
    float clickTime = 0;
    float clickDelay = 0.5f;

    //private Player_Master playerMaster;
    private UnityEngine.AI.NavMeshAgent myNavMeshAgent;

    public bool behindEnemy;
    public static bool guiBusy;

    void Awake()
    {
        SetInitialReferences();
    }

    void SetInitialReferences()
    {
        //stats = GetComponent<Player_Stats>();
        //playerMaster = GetComponent<Player_Master>();
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        attackScript = GetComponent<Player_Attack>();

    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            QuestManager.instance.SetCallbacks();
        }

        // ---- TEMP -----
        if (Input.GetKeyDown(KeyCode.T))
        {
            attackScript.Attack();
        }
        // ---- TEMP -----

        //if (!Player_GUI.isGuiStatsOn)
        //{
        // ---- MOVEMENT WITH KEYBOARD ----
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                
                Vector3 moveDirection = StickToWorldspace(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                // times 3 so that the point it finds is not to close to the player making it stop all the time
                Vector3 destination = transform.position + moveDirection * 1.001f;

                if (destination != Vector3.zero)
                {
                    myNavMeshAgent.Resume();
                    myNavMeshAgent.SetDestination(destination);
                    meleeAttacking = false;
                }
            }


            // ---- MOVEMENT WITH MOUSE ----
            // Move character if has no target and clicked once
            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Interact();
            }

        #region INPUTS

            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000)) {
                // Checks if is an NPC
                NPCController npcController = hit.transform.GetComponent<NPCController>();
                if (npcController != null)
                {
                    npcController.OnClick();
                    myNavMeshAgent.stoppingDistance = 2;
                }
            }
        }
        #endregion

        //If the player has no target and releases the mouse button, the character stops 
        //if (!target || moveWithTarget)
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        moveWithTarget = false;
        //        myNavMeshAgent.Stop();
        //        myNavMeshAgent.SetDestination(transform.position); //makes so the character adopts the stopped position as his new position
        //    }
        //}

        // Attacks attarget if double clicked and target is not null
        if (DoubleClick() && target != null)
            {
                //MoveOrAttack();
                Attack();
            }

            // Keeps attacking the enemy when that flag is true (created for an auto attack)
            if (meleeAttacking)
            {
                Attack();
            }
        //}
    }

    // makes the inputs from keyboard be always relative to the camera
    private Vector3 StickToWorldspace(Vector2 input)
    {
        // Get camera direction
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0; //sets Y to 0 so its on a flat XZ plane (otherwise I would go into the ground since the camera is facing down)

        //find the forward direction based on camera direction
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        // Convert joystick input in worldspace coords
        return (referentialShift * new Vector3(input.x, 0, input.y)).normalized;
    }

    void Interact()
    {
        //DialogueManager.instance.CloseDialogueBox();
        if (target == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Stores the gameobject that received the hit
                GameObject interactedObject = hit.collider.gameObject;

                if(interactedObject.tag == "Interactable Object")
                {
                    interactedObject.GetComponent<Interactable>().MoveToInteractable(myNavMeshAgent);
                }

                else if (!hit.collider.CompareTag("Enemy")) // chase and attack
                {
                    myNavMeshAgent.stoppingDistance = 0;
                    // start moving
                    myNavMeshAgent.Resume();

                    // set the nav mesh destintation
                    myNavMeshAgent.SetDestination(hit.point);

                    meleeAttacking = false;
                    moveWithTarget = true;

                }

                else // move
                {
                    // resets the stoppingDistance since it was set to a different value after clicking an interactable object
                    myNavMeshAgent.stoppingDistance = 0;

                    // set the hit enemy as the players target
                    target = hit.collider.gameObject;
                    targetTransf = hit.collider.transform;

                    //Outline_Golem.enemySelected = true;
                }
            }
        }
        else  // if I try to move somewhere while having a target selected...
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!hit.collider.CompareTag("Enemy")) // chase and attack
                {
                    // start moving
                    myNavMeshAgent.Resume();

                    // set the nav mesh destintation
                    myNavMeshAgent.SetDestination(hit.point);

                    meleeAttacking = false; //flag to make possible for the player to auto attack
                    moveWithTarget = true; //flag to make so that the character stops if the player releases the mouse while having a target selected
                }

            }
        }
    }

    void Attack()
    {
        //if (target)
        //{
        //    // check to see that the target is still valid
        //    if (target.GetComponent<Enemy_Stats>().curHp <= 0)
        //    {
        //        target = null;
        //        targetTransf = null;
        //        meleeAttacking = false;
        //        Outline_Golem.enemySelected = false;
        //        return;
        //    }
        //    else
        //    {
        //        RaycastHit hit;
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        if (Vector3.Distance(transform.position, target.transform.position) <= stats.attackRange) // within attack range
        //        {
        //            // checks to see if the player double clicked in the current target or outside
        //            if (Physics.Raycast(ray, out hit, 100))
        //            {
        //                // if target is withing range and clicked an actual enemy or was previously attacking one already
        //                if (hit.collider.CompareTag("Enemy") || meleeAttacking)
        //                {
        //                    // stop moving
        //                    myNavMeshAgent.Stop();

        //                    // rotate towards the target
        //                    RotateTowards(target.transform);

        //                    // attack!
        //                    attackScript.Attack(target);

        //                    meleeAttacking = true;
        //                }
        //                // clicked inside range but outside an enemy
        //                else
        //                {
        //                    Debug.Log("double clicked outside the enemy, deselect!");
        //                    target = null;
        //                    targetTransf = null;
        //                    meleeAttacking = false;
        //                    Outline_Golem.enemySelected = false;
        //                }
        //            }
        //        }
        //        else // out of attack range
        //        {
        //            // checks to see if the player double clicked in the current target or outside
        //            if (Physics.Raycast(ray, out hit, 100))
        //            {
        //                // if click on a target it was out of range, so pursue to the targets position
        //                if (hit.collider.CompareTag("Enemy"))
        //                {
        //                    Debug.Log("Out of range, but seen!!");
        //                    // start moving
        //                    myNavMeshAgent.Resume();

        //                    // move towards the enemy - set the nav mesh destintation to the targets position
        //                    myNavMeshAgent.SetDestination(target.transform.position);

        //                    meleeAttacking = false;
        //                }
        //                // else the player double cliked outside the enemey and therefore deselects the enemy
        //                else
        //                {
        //                    Debug.Log("double clicked outside the enemy, deselect!");
        //                    target = null;
        //                    targetTransf = null;
        //                    meleeAttacking = false;
        //                    Outline_Golem.enemySelected = false;
        //                }
        //            }
        //        }
        //    }
        //}
    }

    void MoveOrAttack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.CompareTag("Enemy")) // chase and attack
            {
                // set the hit enemy as the players target
                target = hit.collider.gameObject;
                targetTransf = hit.collider.transform;
            }
            else // move
            {

                // remove the current target if one exists
                target = null;
                targetTransf = null;

                // start moving
                myNavMeshAgent.Resume();

                // set the nav mesh destintation
                myNavMeshAgent.SetDestination(hit.point);
            }
        }
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * myNavMeshAgent.speed * 2);
        transform.rotation = lookRotation;
    }

    bool DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
            if (clicked == 1) clickTime = Time.time;
        }
        if (clicked > 1 && Time.time - clickTime < clickDelay)
        {
            Debug.Log("Double CLICK!");
            clicked = 0;
            clickTime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clickTime > 1) clicked = 0;
        return false;
    }

}
