using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour {

    [HideInInspector]   public NavMeshAgent myNavMeshAgent;
    private bool hasInteracted;

    public virtual void MoveToInteractable(NavMeshAgent myNavMeshAgent)
    {
        hasInteracted = false;
        this.myNavMeshAgent = myNavMeshAgent;
        myNavMeshAgent.stoppingDistance = 1.5f;
        myNavMeshAgent.destination = this.transform.position;
    }

    void Update()
    {
        if (myNavMeshAgent != null && !myNavMeshAgent.pathPending)
        {

            if (Vector3.Distance(myNavMeshAgent.transform.position, this.transform.position) > (myNavMeshAgent.stoppingDistance + 5.0f))
            {
                DialogueSystem.Instance.dialoguePanel.SetActive(false);
            }

            if (!hasInteracted)
            {
                if (myNavMeshAgent.remainingDistance < myNavMeshAgent.stoppingDistance)
                {
                    Interact();
                    hasInteracted = true;
                }
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with base class.");
    }
}
