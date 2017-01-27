using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Data {

    public static List<int> finishedQuests = new List<int>();
    public static Dictionary<int, ActiveQuest> activeQuests = new Dictionary<int, ActiveQuest>();
    // Dictionary holding the monsters ids and the number we have killed
    public static Dictionary<int, MonsterKills> monsterKilled = new Dictionary<int, MonsterKills>();

    /// <summary>
    /// Adds the quest to "activeQuests" list
    /// </summary>
    /// <param name="id">Identifier.</param>
    public static void AddQuest(int id)
    {
        // If we already accepted this quest, we wont accept it again
        if (activeQuests.ContainsKey(id)) return;

        // Otherwise, we create a new ActiveQuest
        Quest quest = QuestManager.instance.questDictionary[id];
        ActiveQuest newActiveQuest = new ActiveQuest();
        newActiveQuest.id = id;
        newActiveQuest.dateTaken = DateTime.Now.ToLongDateString();
        // If we need to kill monsters in this quest...
        if(quest.task.kills.Length > 0)
        {
            //set the kills of the new active quest as a new array of length of the kills in the quest
            newActiveQuest.kills = new Quest.QuestKill[quest.task.kills.Length];
            // for every kill in our quest.task
            foreach(Quest.QuestKill questKill in quest.task.kills)
            {
                //set each quest kill to a new instance of questKill
                newActiveQuest.kills[questKill.id] = new Quest.QuestKill();
                // set the player current amount of kills of the new active quest based on the actual
                if (!monsterKilled.ContainsKey(questKill.id)) monsterKilled.Add(questKill.id, new Player_Data.MonsterKills());
                newActiveQuest.kills[questKill.id].initialAmount = monsterKilled[questKill.id].amount;
            }
        }
        activeQuests.Add(id, newActiveQuest);
    }

    // Holds information specific to the instance of this quest. Useful for repeatable quests and counting the number of monster killed
    public class ActiveQuest
    {
        public int id;                   // id of the quest taken
        public string dateTaken;
        public Quest.QuestKill[] kills;  // Holds the task monster ID and the amount of current kills when the quest was accepted
    }

    // How many monsters[id] have we killed in total
    public class MonsterKills
    {
        public int id;
        public int amount;
    }
}
