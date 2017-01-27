using UnityEngine;
using System.Collections;

public class Enemy_Master : MonoBehaviour {

    public Transform myTarget;

    public delegate void GeneralEventHandler();
    public event GeneralEventHandler EventEnemyDie;
    //public event GeneralEventHandler EventEnemyWalking;
    public event GeneralEventHandler EventEnemyReachedNavTarget;
    public event GeneralEventHandler EventEnemyAttack;
    public event GeneralEventHandler EventEnemyLossTarget;

    public delegate void HealthEventHandler(float health);
    public event HealthEventHandler EventEnemyDeductHealth;

    public delegate void NavTargetEventHandler(Transform targetTransform);
    public event NavTargetEventHandler EventEnemySetNavTarget;

    public void CallEventEnemyDeductHealth(float health)
    {
        if (EventEnemyDeductHealth != null)
        {
            EventEnemyDeductHealth(health);
        }
    }

    public void CallEventEnemySetNavTarget(Transform targetTransform)
    {
        if (EventEnemySetNavTarget != null)
        {
            EventEnemySetNavTarget(targetTransform);
        }

        myTarget = targetTransform;
    }

    public void CallEventEnemyDie()
    {
        if (EventEnemyDie != null)
        {
            EventEnemyDie();
        }
    }

    //public void CallEventEnemyWalking()
    //{
    //    if (EventEnemyWalking != null)
    //    {
    //        EventEnemyWalking();
    //    }
    //}

    public void CallEventEnemyAttack()
    {
        if (EventEnemyAttack != null)
        {
            EventEnemyAttack();
        }
    }

    public void CallEventEnemyReachedNavTarget()
    {
        if (EventEnemyReachedNavTarget != null)
        {
            EventEnemyReachedNavTarget();
        }
    }

    public void CallEventEnemyLossTarget()
    {
        if (EventEnemyLossTarget != null)
        {
            EventEnemyLossTarget();
        }

        myTarget = null;
    }
}
