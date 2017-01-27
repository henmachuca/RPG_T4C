using UnityEngine;
using System.Collections;

public class Enemy_Animation : MonoBehaviour
{

    private Enemy_Master enemyMaster;
    private Animator myAnimator;

    private UnityEngine.AI.NavMeshAgent m_agent;

    void Awake()
    {
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (myAnimator && m_agent)
        {
            // update the characters speed based off their navmesh agents velocity magnitude
            myAnimator.SetFloat("Speed", m_agent.velocity.magnitude / m_agent.speed);
        }
    }

    void OnEnable()
    {
        SetInitialReferences();
        enemyMaster.EventEnemyDie += DisableAnimator;
        //enemyMaster.EventEnemyWalking += SetAnimationToWalk;
        enemyMaster.EventEnemyReachedNavTarget += SetAnimationToIdle;
        enemyMaster.EventEnemyAttack += SetAnimationToAttack;
        enemyMaster.EventEnemyDeductHealth += SetAnimationToHit;
    }

    void OnDisable()
    {
        enemyMaster.EventEnemyDie -= DisableAnimator;
        //enemyMaster.EventEnemyWalking -= SetAnimationToWalk;
    }

    void SetInitialReferences()
    {
        enemyMaster = GetComponent<Enemy_Master>();

        if (GetComponent<Animator>() != null)
        {
            myAnimator = GetComponent<Animator>();
        }
    }

    /*
    void SetAnimationToWalk()
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetBool("IsPursuing", true);
            }
        }
    }
    */

    void SetAnimationToIdle()
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetBool("IsPursuing", false);
            }
        }
    }

    void SetAnimationToAttack()
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetTrigger("Attack");
            }
        }
    }

    void SetAnimationToHit(float dummy)
    {
        if (myAnimator != null)
        {
            if (myAnimator.enabled)
            {
                myAnimator.SetTrigger("Hit");
            }
        }
    }

    void DisableAnimator()
    {
        if (myAnimator != null)
        {
            //myAnimator.SetTrigger("Death");
            //myAnimator.enabled = false;
        }
    }
}
