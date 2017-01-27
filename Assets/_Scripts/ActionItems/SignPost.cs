using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : ActionItem {

    public string name;
    public string[] dialogue;            // create a way to add a monologue


    public override void Interact()
    {
        DialogueSystem.Instance.AddNewDialogue(dialogue, name);
        base.Interact();
        Debug.Log("Interacting with sign post!");
    }

}
