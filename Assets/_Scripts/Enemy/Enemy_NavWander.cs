using UnityEngine;
using System.Collections;

public class Enemy_NavWander : MonoBehaviour {

    private Enemy_Master enemyMaster;
    private UnityEngine.AI.NavMeshAgent myNavMeshAgent;
    private float checkRate;
    private float nextCheck;
    private Transform myTransform;
    public float wanderRange;
    private UnityEngine.AI.NavMeshHit navHit;
    private Vector3 wanderTarget;

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
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;
            CheckIfIShouldWander();    
        }

    }

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<Enemy_Master>();
        if(GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        checkRate = Random.Range(1.5f, 2.0f);
        myTransform = transform;
    }

    void CheckIfIShouldWander()
    {
        if (enemyMaster.myTarget == null)
        {
            if (RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
            {
                //enemyMaster.CallEventEnemyWalking();
                myNavMeshAgent.SetDestination(wanderTarget);
            }
        }
    }

    bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * wanderRange;
        if ( UnityEngine.AI.NavMesh.SamplePosition( randomPoint, out navHit, 1.0f, UnityEngine.AI.NavMesh.AllAreas ) )
        {
            result = navHit.position;
            return true;
        }
        else
        {
            result = center;
            return false;
        }
    }

    //draw black sphere over detection radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Use the same vars you use to draw your Overlapsphere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, wanderRange);
    }

    void DisableThis()
    {
        this.enabled = false;
    }
}
