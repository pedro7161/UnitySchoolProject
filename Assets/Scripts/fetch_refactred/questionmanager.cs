using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionmanager : MonoBehaviour
{
    public static quest CurrentQuest = new quest();
    // public static quest CurrentQuest = null;

    public static List<ItemsStructore> allItems = new List<ItemsStructore>();
    public static List<QuestStructure> AllItemsNeeded = new List<QuestStructure>();

    public static List<ItemsQuestStructore> AllquestItems = new List<ItemsQuestStructore>();
    private consumable consumablesScript;
    private InventoryConsumable inventoryConsumableScript;
    public bool HasAllItems = false;

    public static bool QuestCanvasReadyWithItems = false;

    // Start is called before the first frame update
    void Start()
    {
        consumablesScript = FindObjectOfType<consumable>();
        inventoryConsumableScript = FindObjectOfType<InventoryConsumable>();
    }

    void Update() {
        if (CurrentQuest != null && !QuestCanvasReadyWithItems)
        {
            GameObject[] others = GameObject.FindGameObjectsWithTag("Row-Item");
            foreach (GameObject other in others)
            {
                Destroy(other);
            }

            var parent = StateManager.SelectedDialogCanvas.Canvas.gameObject.transform.Find("background");
            var row =  parent?.Find("Row");

            if (row) 
            {
                for(int i = 0; i < questionmanager.CurrentQuest.AllItemsNeeded.Count; i++)
                {
                    var itemRow = Instantiate(row, parent);
                    itemRow.name = "Row-Item";
                    itemRow.gameObject.tag = "Row-Item"; // Identify as UI tag Row-Item

                    // update itemRow to be top 30 and bottom -30
                    var rectTransform = itemRow.GetComponent<RectTransform>();
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, i * -50);

                    itemRow.gameObject.SetActive(true);
                    
                    var item = questionmanager.CurrentQuest.AllItemsNeeded[i];
                    var itemText = itemRow.Find("name");
                    var itemAmount = itemRow.Find("quantity");

                    itemText.GetComponent<TMPro.TextMeshProUGUI>().text = item.ItemName;
                    itemAmount.GetComponent<TMPro.TextMeshProUGUI>().text = $"{item.CurrentAmount}/{item.AmountRequired}";
                }
                QuestCanvasReadyWithItems = true;
            }
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
            else if (currentAmount >= amountRequired)
            {
                HasAllItems = true;
                // Display a warning that the quest is completed
                MissionCompletedWarning();
            }
        }
        QuestCanvasReadyWithItems = false;
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

    }

    public int ItemCollected(int questItem)
    {
        return questItem + 1;
    }

    public void MissionStart(quest quest)
    {
        Debug.Log("Quest started");
        CurrentQuest = quest;
        questionmanager.QuestCanvasReadyWithItems = false;
        // Start the quest in the canvas

        // Enable quest items active to true
        foreach (QuestStructure questItem in quest.AllItemsNeeded)
        {
            foreach(GameObject item in questItem.Item)
            {
                item.SetActive(true);
            }
        }
    }

    public void MissionEnd()
    {
        // Update current quest game object
        CurrentQuest.Isfinished = true;

        // Enable quest items active to true
        foreach (QuestStructure questItem in CurrentQuest.AllItemsNeeded)
        {
            foreach(GameObject item in questItem.Item)
            {
                item.SetActive(false);
            }
        }

        // Removed current quest instance from state
        CurrentQuest = null;
        questionmanager.QuestCanvasReadyWithItems = false;
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
    public void ActivateAllGameObjects()
    {
        GetItems[] getItemsScripts = Resources.FindObjectsOfTypeAll<GetItems>();
    
        foreach (GetItems script in getItemsScripts)
        {
            script.ActivateGameObject();
        }
    }
}