using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionmanager : MonoBehaviour
{
    // public Dictionary<string, string> AllQuests = new Dictionary<string, string>();
    public quest[] AllQuests;
    public static quest CurrentQuest = new quest();
    // public static quest CurrentQuest = null;

    public static List<quest> AllQuestsList = new List<quest>();

    public static List<ItemsStructore> allItems = new List<ItemsStructore>();
    public static List<QuestStructure> AllItemsNeeded = new List<QuestStructure>();

    public static List<ItemsQuestStructore> AllquestItems = new List<ItemsQuestStructore>();

    public itemsquest ItemsQuestScript;
    private consumable consumablesScript;
    private InventoryConsumable inventoryConsumableScript;
    public bool HasAllItems = false;

    public static bool QuestCanvasReadyWithItems = false;

    // Start is called before the first frame update
    void Start()
    {
        consumablesScript = FindObjectOfType<consumable>();
        inventoryConsumableScript = FindObjectOfType<InventoryConsumable>();

        // GetAllMission();
    }

    void Update() {
        if (CurrentQuest != null && StateManager.SelectedMinigame == MinigameType.FETCH_QUEST && !QuestCanvasReadyWithItems)
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

                    itemRow.transform.position = new Vector3(itemRow.transform.position.x, itemRow.transform.position.y - (20 * (i)), itemRow.transform.position.z);
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
            AllQuests = new quest[]
            {
        new quest
        {
            Quest_name = CurrentQuest.Quest_name,
            Quest_description = CurrentQuest.Quest_description,
            Quest_code = CurrentQuest.Quest_code,
            Isfinished = CurrentQuest.Isfinished,
            dificulty = CurrentQuest.dificulty,
            AllItemsNeeded = CurrentQuest.AllItemsNeeded
        }
            };
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
        // Apenas para ser usado na zona de testes
        ActivateAllGameObjects();
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