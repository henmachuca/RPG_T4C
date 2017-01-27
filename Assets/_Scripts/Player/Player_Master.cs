using UnityEngine;
using System.Collections;

public class Player_Master : MonoBehaviour {

    public delegate void GeneralEventHandler();    

    public event GeneralEventHandler EventPlayerDie;
    public event GeneralEventHandler EventPlayerWalking;
    public event GeneralEventHandler EventPlayerReachedNavTarget;
    public event GeneralEventHandler EventPlayerAttack;

    public event GeneralEventHandler EventInventoryChanged;

    public delegate void PlayerHealthEventHandler(int healthChange);
    public event PlayerHealthEventHandler EventPlayerHealthDeduction;
    public event PlayerHealthEventHandler EventPlayerHealthIncrease;

    public void CallEventPlayerDie()
    {
        if (EventPlayerDie != null)
        {
            EventPlayerDie();
        }
    }

    public void CallEventPlayerWalking()
    {
        if (EventPlayerWalking != null)
        {
            EventPlayerWalking();
        }
    }

    public void CallEventPlayerAttack()
    {
        if (EventPlayerAttack != null)
        {
            EventPlayerAttack();
        }
    }

    public void CallEventPlayerReachedNavTarget()
    {
        if (EventPlayerReachedNavTarget != null)
        {
            EventPlayerReachedNavTarget();
        }
    }

    public void CallEventInventoryChanged()
    {
        if (EventInventoryChanged != null)
        {
            EventInventoryChanged();
        }
    }

    public void CallEventPlayerHealthDeduction(int dmg)
    {
        if (EventPlayerHealthDeduction != null)
        {
            EventPlayerHealthDeduction(dmg);
        }
    }

    public void CallEventPlayerHealthIncrease(int dmg)
    {
        if (EventPlayerHealthIncrease != null)
        {
            EventPlayerHealthIncrease(dmg);
        }
    }
}
