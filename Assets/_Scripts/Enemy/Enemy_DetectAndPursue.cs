using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy_DetectAndPursue : MonoBehaviour {

    public LayerMask playerLayer;
    public float detectRadius = 5;

    private NavMeshAgent myNavMeshAgent;
    private Collider[] hitColliders;
    private Enemy_Master enemyMaster;
    private Transform myTransform;
    private float checkRate;
    private float nextCheck;
    private RaycastHit hit;

	
    void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventEnemyDie += DisableThis;
    }

    void OnDisable()
    {
        enemyMaster.EventEnemyDie -= DisableThis;
    }

    void Update()
    {
        CarryOutDetection();
    }

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<Enemy_Master>();
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myTransform = transform;

        checkRate = Random.Range(0.1f, 0.2f);
    }

    // simple detectection of the player by overlapsphere distance
    void CarryOutDetection()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            hitColliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);

            if (hitColliders.Length > 0 && hitColliders[0].CompareTag("Player")) //if the overlapsphere found any hitpoints and it belongs to the player tag
            {
                //Debug.Log("Enemy found player");
                enemyMaster.myTarget = hitColliders[0].transform;   //adds the player as the enemy master target
                myNavMeshAgent.SetDestination(hitColliders[0].transform.position);
                //enemyMaster.CallEventEnemySetNavTarget(hitColliders[0].transform);
            }

            else
            {
                //Enemy lost target!
                //enemyMaster.CallEventEnemyLossTarget();
            }
        }
    }

    //draw black sphere over detection radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        //Use the same vars you use to draw your Overlapsphere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    void DisableThis()
    {
        this.enabled = false;
    }
}
