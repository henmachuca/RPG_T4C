using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour {

    private Enemy_Master enemyMasterScript;
    private Transform myTransform;
    private Transform attackTarget;


    public Animator myAnim;
    private bool dead;
    public int monsterId;
    public float totalHealth;
    public float currentHealth;
    public int expGranted;
    public float atkDamage;
    public float attackRange;
    public float attackRate;
    public float nextAttack;
    public float moveSpeed;

    public Outline outlineScript;  //stores the Outline script to activate it when mouse is over the monster

    private GameObject[] players;    // array to store players who deal damage to a mob! Useful for parties for example and split exp.

    // Use this for initialization
    void Start () {
        enemyMasterScript = GetComponent<Enemy_Master>();
        myAnim = GetComponent<Animator>();
        currentHealth = totalHealth;
        myTransform = transform;
        players = GameObject.FindGameObjectsWithTag("Player");
        
        //Outline
        outlineScript = transform.GetChild(0).GetComponent<Outline>();
        outlineScript.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        TryToAttack();
    }

    #region Creates OUTLINES
    private void OnMouseOver()
    {
        outlineScript.enabled = true;
    }

    private void OnMouseExit()
    {
        outlineScript.enabled = false;
    }
    #endregion

    public void GetHit(float damage)
    {
        if (dead) return;       //if the enemy is dead it will not run the other functions!
        myAnim.SetTrigger("Hit");
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Dead();
            enemyMasterScript.CallEventEnemyDie(); //Makes so the event gets called to all other scripts like the DetectPursue!
        }
    }

    public void SetAttackTarget(Transform targetTransform)
    {
        attackTarget = targetTransform;
    }

    void TryToAttack()
    {
        if(enemyMasterScript.myTarget != null && enemyMasterScript.myTarget.CompareTag("Player"))
        {
            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                if(Vector3.Distance(myTransform.position, enemyMasterScript.myTarget.position) < attackRange)
                {
                    Vector3 lookAtVector = new Vector3(enemyMasterScript.myTarget.position.x, myTransform.position.y, enemyMasterScript.myTarget.position.z);
                    myTransform.LookAt(lookAtVector);
                    Debug.Log("ATTACK");
                    enemyMasterScript.myTarget.GetComponent<Player_Attack>().GetHit(10);
                }
            }
        }
    }

    void Dead()
    {
        if (!Player_Data.monsterKilled.ContainsKey(monsterId)) Player_Data.monsterKilled.Add(monsterId, new Player_Data.MonsterKills());
        //Increase the amount of times we have killed this monster[id]
        Player_Data.monsterKilled[monsterId].amount++;

        //Set dead and do the rest
        dead = true;
        DropLoot();
        foreach (GameObject go in players)
        {
            go.GetComponent<Player_Attack>().SetExperience(expGranted/players.Length);
        }
        myAnim.SetBool("Dead", true);
        GameObject.Destroy(this.gameObject, 3);
    }

    void DropLoot()
    {
        Debug.Log("Get the Loot!!");
    }

    // THIS METHOD WHEN ACTIVATING THE COLLIDER IN THE WEAPON CHECKS FOR COLLISION IN CONTACT
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Attack hit the Collider");
    }

}
