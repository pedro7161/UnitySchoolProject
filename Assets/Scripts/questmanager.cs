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
    Debug.Log("Trying to start quest: " + questId);

    // Check if the quest is valid and can be started
    if (!IsQuestValid(questId))
    {
        Debug.LogWarning("Invalid quest or quest already started.");
        return;
    }

    // Update the state of the quest or perform other necessary actions
    // ...

    // Trigger the quest started event
    OnQuestStarted?.Invoke(questId);

    Debug.Log("Quest started: " + questId);
}

private bool IsQuestValid(int questId)
{
    // Add your implementation to check if the quest is valid and can be started
    // Return true if the quest is valid, or false otherwise
    // You can check for conditions such as whether the quest has already been started or if the questId exists in your quest management system
    // Example:
    // return questDatabase.Contains(questId) && !IsQuestStarted(questId);

    // For simplicity, let's assume all quests are valid in this example
    return true;
}
}
