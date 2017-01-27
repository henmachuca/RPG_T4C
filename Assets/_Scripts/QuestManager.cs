using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    public static QuestManager instance;

    public Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();

	void Awake () {
        if (instance == null) instance = this;
        LoadQuests();
	}
	
	void LoadQuests()
    {
        Quest newQuest = JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("Json Files/Quest_Json").text);
        questDictionary.Add(newQuest.id, newQuest);
    }

    public void SetCallbacks()
    {
        InputManager.KeyPressedDown += KeyCallbacks;
    }

    void KeyCallbacks() {
        Debug.Log("KeyCallBacks: "+!UIController.instance.questBook.gameObject.activeInHierarchy);
        if (Input.inputString == "b") {
            ToggleQuestBook(!UIController.instance.questBook.gameObject.activeInHierarchy);
        }
    }

    void ToggleQuestBook(bool b) {
        UIController.instance.questBook.gameObject.SetActive(b);
        if(b) ShowActiveQuest();
    }

    //What shows in the quest book
    public void ShowActiveQuest() {
        foreach(Player_Data.ActiveQuest activeQuest in Player_Data.activeQuests.Values) {
            int i = activeQuest.id;
            if(UIController.instance.questBookContent.Find(i.ToString()) != null) {
                continue;  // If we found this quest ID as one of the children of questBookContent, we skip the creation of this button
            }
            // Create new quest button
            GameObject QuestButtonGo = Instantiate(Resources.Load("Prefabs/Quest_Button_Prefab") as GameObject);
            QuestButtonGo.name = questDictionary[i].id.ToString();
            QuestButtonGo.transform.SetParent(UIController.instance.questBookContent);
            QuestButtonGo.transform.localScale = Vector3.one;
            QuestButtonGo.transform.Find("Text").GetComponent<Text>().text = questDictionary[i].questName;
            int questId = new int();
            questId = i;
            QuestButtonGo.GetComponent<Button>().onClick.AddListener(() => {
                ShowQuestInfo(questDictionary[questId]);
            });
        }
    }

    public void ShowQuestInfo(Quest quest)
    {
        // Show quest info panel
        UIController.instance.questInfo.gameObject.SetActive(true);
        // Show/Hide AcceptButton depending on "Do we have this quest?"
        UIController.instance.questInfoAcceptButton.gameObject.SetActive(!Player_Data.activeQuests.ContainsKey(quest.id));

        // Hide the COMPLETE button. It will be opened by the npc if appropriate
        UIController.instance.questInfoCompleteButton.gameObject.SetActive(false);

        // Remove previous functions from Accept Button
        UIController.instance.questInfoAcceptButton.onClick.RemoveAllListeners();
        // Set function on ACCEPT button
        UIController.instance.questInfoAcceptButton.onClick.AddListener(() =>
        {
            Player_Data.AddQuest(quest.id);
            UIController.instance.questInfo.gameObject.SetActive(false);
            ShowActiveQuest();
        });
        // Set Texts
        UIController.instance.questInfoContent.Find("Name").GetComponent<Text>().text = quest.questName;
        UIController.instance.questInfoContent.Find("Description").GetComponent<Text>().text = quest.description;
        // TASKS
        string taskString = "Task:\n";
        if (quest.task.kills != null)
        {
            foreach (Quest.QuestKill qk in quest.task.kills)
            {
                // current kills is 0 when we havent taken the quest
                int currentKills = 0;
                if (Player_Data.activeQuests.ContainsKey(qk.id))
                    // if we are showing the info during the progress of the quest (we took it already) show the progress
                    // the amount of monsters killed - the times i killed that monster when i took that quest
                    currentKills = Player_Data.monsterKilled[qk.id].amount - Player_Data.activeQuests[quest.id].kills[qk.id].initialAmount;

                taskString += "Slay " + (currentKills) + "/" + qk.amount + " " + MonsterDatabase.monsters[qk.id] + ".\n";
            }
        }

        if (quest.task.items != null)
        {
            foreach (Quest.QuestItem qi in quest.task.items)
            {
                taskString += "Bring " + qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
            }
        }
        if (quest.task.talkTo != null)
        {
            foreach (int id in quest.task.talkTo)
            {
                taskString += "Talk to " + NPCDatabase.npcs[id] + ".\n";
            }
        }
        UIController.instance.questInfoContent.Find("Task").GetComponent<Text>().text = taskString;

        // REWARDS
        string rewardString = "Reward: \n";

        if (quest.reward.items != null)
        {
            foreach (Quest.QuestItem qi in quest.reward.items)
            {
                rewardString += qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
            }
        }

        if (quest.reward.exp > 0) rewardString += quest.reward.exp + "Experience.\n";
        if (quest.reward.money > 0) rewardString += quest.reward.money + "Gold.\n";
        UIController.instance.questInfoContent.Find("Reward").GetComponent<Text>().text = rewardString;
    }

    public bool IsQuestAvailable(int questId, Player_Attack player){
        return (questDictionary[questId].requiredLevel <= player.level);
    }

    public bool IsQuestFinished(int questId)
    {
        Quest quest = questDictionary[questId];
        // Check kills
        // if there is at least one kill that we are required to do.
        if (quest.task.kills.Length > 0)
        {
            // foreach kill that we must do
            foreach (var questKill in quest.task.kills)
            {
                if(!Player_Data.monsterKilled.ContainsKey(questKill.id))
                {
                    return false;
                }
                int currentKills = Player_Data.monsterKilled[questKill.id].amount - Player_Data.activeQuests[quest.id].kills[questKill.id].initialAmount;
                if (currentKills < questKill.amount)
                {
                    return false;
                }
            }
        }
        //Do the same but check Items on inventory
        //If we dont have the required items at any point, return false

        // Same for Talked to. Return false if complete

        // If at any point the quest is incomplete, we would have return false and stop running
        // Since we reach this point, the quest is complete, so we return true
        return true;
    }
}
