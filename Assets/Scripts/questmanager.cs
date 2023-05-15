using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questmanager : MonoBehaviour
{
 public static questmanager instance;  // Singleton instance of the QuestManager

    // Add variables and methods to manage quests
    // For example, you could have a list of active quests and methods to start/complete quests.
   

    // Add variables and methods to manage quests
    // For example, you could have a list of active quests and methods to start/complete quests.

    // Event for quest start
    public delegate void QuestStartedDelegate(int questId);
    public event QuestStartedDelegate OnQuestStarted;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void StartQuest(int questId)
{
    // Implementation of starting a quest goes here
    // You can update the state of the quest or perform any other necessary actions
}
}
