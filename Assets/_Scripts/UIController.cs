using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController instance;
    public Transform canvas;
    //Quest Info
    public Transform questInfo;
    public Transform questInfoContent;
    public Button questInfoAcceptButton;
    public Button questInfoCompleteButton;
    public Button questInfoCancelButton;
    //Active Quest Book
    public Transform questBook;
    public Transform questBookContent;


    // Changed from Start to Awake so it loads before everything else and gives no errors for me.
	void Awake () {
        if (!instance) instance = this;
        canvas = GameObject.Find("Canvas").transform;
        questInfo = canvas.Find("Quest Info");
        questInfoContent = questInfo.Find("Quest Info Background/Info/Viewport/Content");
        questBook = canvas.Find("Quest Book");
        questBookContent = canvas.Find("Quest Book/Quest Book Background/Info/Viewport/Content");
        questInfoAcceptButton = questInfo.Find("Quest Info Background/Buttons/Accept").GetComponent<Button>();
        questInfoCompleteButton = questInfo.Find("Quest Info Background/Buttons/Complete").GetComponent<Button>();
        questInfoCancelButton = questInfo.Find("Quest Info Background/Buttons/Cancel").GetComponent<Button>();
        questInfoCancelButton.onClick.AddListener(() => { questInfo.gameObject.SetActive(false); });

    }

    //public void ShowQuestInfo(Quest quest)
    //{
    //    Transform info = GameObject.Find("Canvas/Quest Info/Quest Info Background/Info/Viewport/Content").transform;
    //    info.Find("Name").GetComponent<Text>().text = quest.questName;
    //    info.Find("Description").GetComponent<Text>().text = quest.description + "\n";
    //    // TASK
    //string taskString = "Task :\n";

    //    if (quest.task.kills != null)
    //    {
    //        foreach (Quest.QuestKill qk in quest.task.kills)
    //        {
    //            taskString += "Slay " + qk.amount + " " + MonsterDatabase.monsters[qk.id] + ".\n";
    //        }
    //    }

    //    if (quest.task.items != null)
    //    {
    //        foreach (Quest.QuestItem qi in quest.task.items)
    //        {
    //            taskString += "Bring " + qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
    //        }
    //    }
    //    if (quest.task.talkTo != null)
    //    {
    //        foreach (int id in quest.task.talkTo)
    //        {
    //            taskString += "Talk to " + NPCDatabase.npcs[id] + ".\n";
    //        }
    //    }

    //    info.Find("Task").GetComponent<Text>().text = taskString;

    //    //REWARD
    //    string rewardString = "Reward: \n";

    //    if (quest.reward.items != null)
    //    {
    //        foreach (Quest.QuestItem qi in quest.reward.items)
    //        {
    //            rewardString += qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
    //        }
    //    }

    //    if (quest.reward.exp > 0) rewardString += quest.reward.exp + "Experience.\n";
    //    if (quest.reward.money > 0) rewardString += quest.reward.money + "Gold.\n";

    //    info.Find("Reward").GetComponent<Text>().text = rewardString;
    //}
}
