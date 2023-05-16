using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questmanager : MonoBehaviour
{
    public static questmanager instance;  // Singleton instance of the QuestManager

    // Dictionary to store the active quests and their required item counts
    private Dictionary<int, int> activeQuests = new Dictionary<int, int>();

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

    // Method to start a quest with a specified required item count
    public void StartQuest(int questId, int requiredItemCount)
    {
        Debug.Log("Trying to start quest: " + questId);

        // Check if the quest is valid and can be started
        if (!IsQuestValid(questId))
        {
             Debug.Log("Invalid quest or quest already started. " + questId);
            Debug.LogWarning("Invalid quest or quest already started.");
            return;
        }

        // Update the state of the quest or perform other necessary actions
        // ...

        // Set the required item count for the quest
        activeQuests[questId] = requiredItemCount;

        // Trigger the quest started event
        OnQuestStarted?.Invoke(questId);

        Debug.Log("Quest started: " + questId);
    }

    // Method to check if a quest is valid and can be started
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

    // Method to check if a quest is active
    public bool IsQuestActive(int questId)
    {
        return activeQuests.ContainsKey(questId);
    }

    // Method to update the quest progress when an item is fetched
    public void UpdateQuestProgress(int questId, int itemId)
    {
        // Check if the quest is active
        if (IsQuestActive(questId))
        {
            // Decrease the required item count for the quest
            activeQuests[questId]--;

            // Perform additional actions based on the updated quest progress
            // ...
        }
    }

    // Method to get the required item count for a quest
    public int GetRequiredItemCount(int questId)
    {
        if (activeQuests.ContainsKey(questId))
        {
            return activeQuests[questId];
        }

        return 0;
    }

    // Method to complete a quest
    public void CompleteQuest(int questId)
    {
        // Add your implementation to complete a quest
        // Perform necessary actions when a quest is completed
        // ...

        // Remove the quest from the active quests dictionary
        activeQuests.Remove(questId);
        inventory.instance.RemoveItem(questId);
    }

    // Method to check if an item is still needed for a quest
   public bool IsItemStillNeeded(int itemId)
{
    foreach (int questId in activeQuests.Keys)
    {
        if (inventory.instance.HasItem(questId) && activeQuests[questId] > 0)
        {
            return false; // Item is still needed for at least one active quest
        }
    }

    return true; // Item is not needed for any active quest
}
}
