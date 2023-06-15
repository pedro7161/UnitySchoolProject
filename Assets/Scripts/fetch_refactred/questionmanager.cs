using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private static List<GameObject> currentQuestRowItems = new List<GameObject>();
    public static bool shouldStartQuestAutomatically = false;

    public static bool shouldPlayQuestCompleteSound = true;

    // Start is called before the first frame update
    void Start()
    {
        consumablesScript = FindObjectOfType<consumable>();
        inventoryConsumableScript = FindObjectOfType<InventoryConsumable>();
    }

    void Update()
    {
        if (!QuestCanvasReadyWithItems)
        {
            foreach (GameObject other in currentQuestRowItems)
            {
                Destroy(other);
            }
        }
        if (CurrentQuest != null && CurrentQuest.AllItemsNeeded.TrueForAll(x => x.IsCompleted))
        {
            var canvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.QUEST);
            if (canvas == null)
            {
                return;
            }
            var parent = canvas.Canvas?.gameObject?.transform?.Find("background");
            if (parent != null)
            {
                parent.Find("QuestInformation").GetComponentInChildren<TextMeshProUGUI>().text = "Nice! Please interact with the terminal to complete the quest.";
                if (shouldPlayQuestCompleteSound)
                {
                    StartCoroutine(
                        Config.Waiter(null, () => {
                            var levelManagerInstance = GameObject.Find("LevelManager").GetComponent<LevelManager>();
                            levelManagerInstance.soundEffects.Find(sound => sound.soundEffect == SoundEffectEnum.QUEST_COMPLETE).audioSource.Play();
                            shouldPlayQuestCompleteSound = false;
                        }, 0.25f)
                    );
                }
            }
        }
        if (CurrentQuest != null && !QuestCanvasReadyWithItems)
        {
            currentQuestRowItems.Clear();
            // get quest canvas
            var canvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.QUEST);
            if (canvas == null)
            {
                return;
            }

            var parent = canvas.Canvas.gameObject.transform.Find("background");
            var row = parent?.Find("Row");
            parent.Find("QuestInformation").GetComponentInChildren<TextMeshProUGUI>().text = "";

            if (row)
            {
                for (int i = 0; i < questionmanager.CurrentQuest.AllItemsNeeded.Count; i++)
                {
                    var itemRow = Instantiate(row, parent);
                    itemRow.name = "QuestRowItem";
                    itemRow.gameObject.tag = "QuestRowItem"; // Identify as UI tag Row-Item

                    // update itemRow to be top 30 and bottom -30
                    var rectTransform = itemRow.GetComponent<RectTransform>();
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, i * -50);

                    itemRow.gameObject.SetActive(true);

                    var item = questionmanager.CurrentQuest.AllItemsNeeded[i];
                    var itemText = itemRow.Find("name");
                    var itemAmount = itemRow.Find("quantity");

                    itemText.GetComponent<TMPro.TextMeshProUGUI>().text = item.ItemName;
                    itemAmount.GetComponent<TMPro.TextMeshProUGUI>().text = $"{item.CurrentAmount}/{item.AmountRequired}";

                    currentQuestRowItems.Add(itemRow.gameObject);
                }
                QuestCanvasReadyWithItems = true;
            }
        }
    }

    public void UpdateQuest()
    {
        Debug.Log("test - UpdateQuest() called" + LevelManager.GetCurrentLevel()?.currentQuest);
        var currentQuest = GameObject.Find(LevelManager.GetCurrentLevel().currentQuest).GetComponent<quest>();
        foreach (QuestStructure questItem in currentQuest.AllItemsNeeded)
        {
            int currentAmount = questItem.CurrentAmount;
            int amountRequired = questItem.AmountRequired;
            var levelManagerInstance = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            levelManagerInstance.soundEffects.Find(sound => sound.soundEffect == SoundEffectEnum.ITEM_COLLECT).audioSource.Play();

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
        CurrentQuest = quest;
        questionmanager.QuestCanvasReadyWithItems = false;
        // Start the quest in the canvas

        // Enable quest items active to true
        foreach (QuestStructure questItem in quest.AllItemsNeeded)
        {
            foreach (GameObject item in questItem.Item)
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
            foreach (GameObject item in questItem.Item)
            {
                item.SetActive(false);
            }
        }

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

    public string humanizeMinigameName(MinigameType minigameType)
    {
        string minigameName = minigameType.ToString();

        minigameName = minigameName.Replace("_", " ").ToLower();
        minigameName = char.ToUpper(minigameName[0]) + minigameName.Substring(1);
        return minigameName;
    }
}