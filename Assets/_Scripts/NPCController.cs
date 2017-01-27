using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {

    [SerializeField] int[] quests;          // quests the NPC will have available for the character
    [SerializeField] string[] dialogues;    // The dialogue steps
    public int dialogueIndex = 0;           // Keeps track of where the player is at in a quest for example

    private Quest quest;

	// Use this for initialization
	void Start () {
        //UIController.instance.ShowQuestInfo(QuestManager.instance.questDictionary[0]);
	}

    public void ShowDialogue() {
        if (dialogueIndex > (dialogues.Length - 1)) {
            DialogueManager.instance.CloseDialogueBox();
            dialogueIndex = (dialogues.Length - 2);
        }
        else
        {
            DialogueManager.instance.PrintOnDialogueBox(name + ":" + dialogues[dialogueIndex]);
        }
    }

    public void ShowQuestInfo()
    {
        foreach (int i in quests)
        {
            if (
                // Did the player finished this quest?
                !Player_Data.finishedQuests.Contains(i) &&
                // Does the player meet the requirements?
                QuestManager.instance.IsQuestAvailable(i, GameObject.Find("Player").GetComponent<Player_Attack>())
                ) {
                // Show the info of the quest
                QuestManager.instance.ShowQuestInfo(QuestManager.instance.questDictionary[quests[i]]);

                // Set the Complete quest button
                if (QuestManager.instance.IsQuestFinished(i))
                {
                    UIController.instance.questInfoCompleteButton.gameObject.SetActive(true);
                    UIController.instance.questInfoCompleteButton.onClick.AddListener(() => {
                        ReceiveCompletedQuest(QuestManager.instance.questDictionary[quests[i]]);
                        Player_Data.activeQuests.Remove(i);
                        Player_Data.finishedQuests.Add(i);
                        UIController.instance.questInfoCompleteButton.onClick.RemoveAllListeners();
                        UIController.instance.questInfo.gameObject.SetActive(false);
                        // TO DO: Remove the quest button from the quest book!
                        
                    });
                }
                
                break;
            }
        }
    }

    void ReceiveCompletedQuest(Quest quest) {
        if (quest.reward.exp > 0) Player_Attack.main.SetExperience(quest.reward.exp);
        if (quest.reward.items.Length > 0)
        {
            foreach (var item in quest.reward.items)
            {
                print("You get: (" + item.amount + ")x" + ItemDatabase.items[item.id]);
                //ex. inventory.add(item.id, item.amount);
            }
        }
    }

    public void OnClick()
    {
        ShowQuestInfo();
    }

    //void SetQuestExample()
    //{
    //    quest = new Quest();
    //    quest.questName = "Mummies Everywhere!";
    //    quest.description = "These mummies are annoying. Kill'em and you will get something nice!";
    //    // Reward
    //    quest.reward = new Quest.Reward();
    //    quest.reward.exp = 400;
    //    quest.reward.items = new Quest.QuestItem[1];
    //    quest.reward.items[0] = new Quest.QuestItem();
    //    quest.reward.items[0].id = 2;
    //    quest.reward.items[0].amount = 1;
    //    // Task
    //    quest.task = new Quest.Task();
    //    quest.task.kills = new Quest.QuestKill[2];
    //    quest.task.kills[0] = new Quest.QuestKill();
    //    quest.task.kills[0].id = 0;
    //    quest.task.kills[0].amount = 10;
    //    quest.task.kills[1] = new Quest.QuestKill();
    //    quest.task.kills[1].id = 1;
    //    quest.task.kills[1].amount = 2;
    //}
}
