using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;

    private Transform dialogueBox;
    private Text dialogueBoxTextBody;
    
	void Start () {
        if (instance == null) instance = this;

        dialogueBox = UIController.instance.canvas.Find("Dialogue Box");
        dialogueBoxTextBody = dialogueBox.Find("DialogueBoxBodyText").GetComponent<Text>();       	
	}
	
    public void PrintOnDialogueBox(string text)
    {
        dialogueBox.gameObject.SetActive(true);
        dialogueBoxTextBody.text = text;
        InputManager.OnPressUp += CloseDialogueBoxCallback;
    }

    public void CloseDialogueBox()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    public void CloseDialogueBoxCallback()
    {
        CloseDialogueBox();
        InputManager.OnPressUp -= CloseDialogueBoxCallback;
    }
}
