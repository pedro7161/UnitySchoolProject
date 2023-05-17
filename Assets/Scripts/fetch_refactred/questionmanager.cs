using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionmanager : MonoBehaviour
{
    public Dictionary<string, string> AllQuests = new Dictionary<string, string>();
    public static quest CurrentQuest = new quest();
    // public static quest CurrentQuest = null;

    public static List<ItemsStructore> allItems = new List<ItemsStructore>();
    public static List<QuestStructure> AllItemsNeeded = new List<QuestStructure>();

    public static List<ItemsQuestStructore> AllquestItems = new List<ItemsQuestStructore>();

    public itemsquest ItemsQuestScript;
    private consumable consumablesScript;
    private InventoryConsumable inventoryConsumableScript;


    // Start is called before the first frame update
    void Start()
    {
        consumablesScript = FindObjectOfType<consumable>();
        inventoryConsumableScript = FindObjectOfType<InventoryConsumable>();

        // GetAllMission();
    }

    public void GetAllMission()
    {
        foreach (ItemsQuestStructore questItem in AllquestItems)
        {
            if (questItem.CurrentAmount >= questItem.AmountRequired)
            {
                CurrentQuest.Isfinished = true;
            }
        }

        if (CurrentQuest.Isfinished)
        {
            AllQuests.Add(CurrentQuest.Quest_name, CurrentQuest.Quest_description + " Finished");
        }
        else if (!CurrentQuest.Isfinished)
        {
            AllQuests.Add(CurrentQuest.Quest_name, CurrentQuest.Quest_description + " Unfinished");
            
        }
        else if(CurrentQuest == null)
        {
            AllQuests.Add("No Quest", "No Quest" + " Unfinished");
        }
        else
        {
            return;
        }
    }

    public void UpdateQuest()
    {
        foreach (ItemsQuestStructore questItem in AllquestItems)
        {
            int currentAmount = questItem.CurrentAmount;
            int amountRequired = questItem.AmountRequired;

            if (currentAmount < amountRequired)
            {
                // Update collected items for the quest in the canvas
                UpdateCollectedItems(currentAmount);
            }
        }
    }

    private void UpdateCollectedItems(int currentAmount)
    {
        // Perform actions to update collected items in the canvas
        // ...
    }

    public void MissionCompletedWarning()
    {
        Debug.Log("Quest completed warning");
        // Display a warning that the quest is completed
    }

    public void MissionCompleted()
    {
        CurrentQuest.Isfinished = true;
        GetAllMission();
    }

    public int ItemCollected(int questItem)
    {
        return questItem + 1;
    }

    public void MissionStart(quest quest)
    {
        Debug.Log("Quest started");
        CurrentQuest = quest;
        // Start the quest in the canvas
    }

    public void MissionEnd()
    {
        Debug.Log("Quest ended");
        CurrentQuest = null;
    }
    // Codigo a ver com consumiveis ainda a ser pensado
    // Example method to access the variables from consumable script
    public void CheckConsumableType()
    {
        bool isBabyMode = consumablesScript.IsBabyMode;
        bool isInstantSolver = consumablesScript.IsIstantSolver;
        bool isTimeUpper = consumablesScript.IsTimeUpper;

        // Use the variables as needed
        // ...
    }

    // Example method to access the variables from InventoryConsumable script
    public void CheckAmount()
    {
        int amount = inventoryConsumableScript.Amount;
        bool wasConsumed = inventoryConsumableScript.WasConsumed;
        // Use the variables as needed
        // ...

    }


}