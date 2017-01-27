using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {

    public static DialogueSystem Instance { get; set; }

    public GameObject dialoguePanel;  // reference to the UI element
    public string objectName;
    public List<string> dialogueLines = new List<string>();

    Button continueButton;
    Text dialogueText, nameText;
    int dialogueIndex;

    void Awake ()
    {
        continueButton = dialoguePanel.transform.FindChild("Continue").GetComponent<Button>();
        dialogueText = dialoguePanel.transform.FindChild("Text").GetComponent<Text>();
        nameText = dialoguePanel.transform.FindChild("Name").GetChild(0).GetComponent<Text>();
        continueButton.onClick.AddListener(delegate { ContinueDialogue(); } );

        dialoguePanel.SetActive(false);

		if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else //an instance does not exist
        {
            // a reference to this object the one we created by dragging and dropping the component on a gameobject in the hierarchy
            Instance = this;
        }
	}
	
    public void AddNewDialogue(string[] lines, string objectName)
    {
        dialogueIndex = 0;
        dialogueLines = new List<string>(lines.Length);
        // adds to the List array all the .length without the need to use for each!
        dialogueLines.AddRange(lines);

        this.objectName = objectName;
        Debug.Log(dialogueLines.Count);
        CreateDialogue();
    }

    public void CreateDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = objectName;
        dialoguePanel.SetActive(true);
    }

    public void ContinueDialogue()
    {
        if (dialogueIndex < dialogueLines.Count - 1)
        {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}
