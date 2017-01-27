using UnityEngine;
using System.Collections;

public class Player_Animations : MonoBehaviour {

    private Player_Master playerMaster;
    private Animator myAnimator;

    private UnityEngine.AI.NavMeshAgent m_agent;

    // ALL EVENTS ARE CREATED HERE
    public delegate void AnimationEvent();
    public static event AnimationEvent OnSlashAnimationHit;

    void Awake()
    {
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void OnEnable()
    {
        SetInitialReferences();
        //playerMaster.EventPlayerDie += DisableAnimator;
        //playerMaster.EventPlayerAttack += SetAnimationToAttack;
    }

    void OnDisable()
    {
        //playerMaster.EventPlayerDie -= DisableAnimator;
    }

    void Update()
    {
        if (myAnimator && m_agent)
        {
            // update the characters speed based off their navmesh agents velocity magnitude
            myAnimator.SetFloat("Speed", m_agent.velocity.magnitude / m_agent.speed);
        }
    }

    void SetInitialReferences()
    {
        playerMaster = GetComponent<Player_Master>();

        if (GetComponent<Animator>() != null)
        {
            myAnimator = GetComponent<Animator>();
        }        
    }

    void SlashAnimationHitEvent()
    {
        print("Event Called");
        OnSlashAnimationHit();
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

    void DisableAnimator()
    {
        if (myAnimator != null)
        {
            myAnimator.enabled = false;
        }
    }
}
