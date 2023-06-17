using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]

public class QuestStructure
{
    public List<GameObject> Item;
    public int CurrentAmount;
    public int AmountRequired;
    public bool IsCompleted;
    public string ItemName;
    public bool isMinigameQuest = false;
    public MinigameType MinigameName = MinigameType.NONE;
}

public class quest : MonoBehaviour
{
    public string Quest_name;
    public string Quest_description;
    public string Quest_code;
    public bool Isfinished = false;
    public enum Dificulty
    {
        Easy,
        Medium,
        Hard
    }
    public Dificulty dificulty;
    public List<QuestStructure> AllItemsNeeded;

    private questionmanager questionmanager = new questionmanager();
    public bool PlayerEnteredOnCollider = false;

    public bool AllMissionsCompleted = false;

    public bool shouldStartQuestProgramatically = false;

    public static bool playerCanInteract = false;

    private void Start()
    {
        StartCoroutine(Config.Waiter(() => {
            playerCanInteract = false;
        }, () => {
            playerCanInteract = true;
        }, 1.5f));
        if (AllItemsNeeded != null)
        {
            questionmanager.AllItemsNeeded = AllItemsNeeded;
        }
    }

    public void StartQuest()
    {
        shouldStartQuestProgramatically = false;
        questionmanager.shouldPlayQuestCompleteSound = true;
        questionmanager.MissionStart(this);
        StateManager.SetupDialog(new List<string>(), DialogType.QUEST, false);
    }

    private void Update()
    {
        if (StateManager.isDialogRunning && StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.ALERT) != null)
        {
            return;
        }
        Debug.Log("test " + questionmanager.CurrentQuest + " " + AllMissionsCompleted + " " + playerCanInteract);
        if (PlayerEnteredOnCollider && Input.GetKey(KeyCode.R) && playerCanInteract || shouldStartQuestProgramatically)
        {
            StartCoroutine(Config.Waiter(() => {
                playerCanInteract = false;
            }, () => {
                playerCanInteract = true;
            }, 1.5f));

            if ((questionmanager.CurrentQuest == null) && !AllMissionsCompleted)
            {
                LevelManager.StartQuestLevel();
            }

            else if (AllItemsCompleted())
            {
                if (AllMissionsCompleted)
                {
                    StateManager.chatCanvasShouldRender = false;
                    StateManager.OnStopDialog();

                    var questCanvas = GameObject.Find("QuestCanvas");
                    if (questCanvas != null)
                    {
                        questCanvas.SetActive(false);
                    }
                    Debug.Log("test - Setting up dialog");
                    StateManager.SetupDialog(new List<string>{"Quest Completed!"}, DialogType.ALERT, false);
                }
                else
                {
                    Debug.Log("test - All Items completed");
                    AllMissionsCompleted = true;
                    questionmanager.MissionEnd();
                    questionmanager.CurrentQuest = null;
                    StateManager.OnStopDialog();
                }

            }
            else if (questionmanager.CurrentQuest != null && !questionmanager.CurrentQuest.Isfinished && questionmanager.CurrentQuest != null)
            {
                // Display a message or take appropriate action to indicate that a quest is already in progress
            }
            else if (questionmanager.CurrentQuest != null && questionmanager.CurrentQuest.Isfinished)
            {
                var levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
                var quests = levelManager.levels.Find(x => x.level == LevelManager.GetCurrentLevel().level).questsGameObject;

                var allQuestionsAreCompleted = true;
                var questsStructure = Resources.FindObjectsOfTypeAll<quest>().ToList().Where(x => quests.Contains(x.gameObject.name));
                foreach (quest quest in questsStructure)
                {
                    allQuestionsAreCompleted = allQuestionsAreCompleted && quest.Isfinished;
                }

                if (allQuestionsAreCompleted)
                {
                    StateManager.OnStopDialog();
                }
            }
            else
            {
            }
        }
        else if (questionmanager.HasAllItems && PlayerEnteredOnCollider)
        {
            questionmanager.MissionCompleted();
        }
        else
        {
            // Debug.Log("Player is not on collider");
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerEnteredOnCollider = true;

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerEnteredOnCollider = false;
    }

    private void UpdateQuest(Collision collision)
    {
        foreach (QuestStructure questItem in AllItemsNeeded)
        {
            if (questItem.Item.Contains(collision.gameObject))
            {
                questItem.CurrentAmount++;
                if (questItem.CurrentAmount >= questItem.AmountRequired)
                {
                    questItem.IsCompleted = true;
                    // Perform any necessary actions for quest completion here
                }
            }
        }
    }

    public void ResetAllQuestsInMap() {
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Quest"))
        {
            quest quest = gameObject.GetComponent<quest>();
            foreach(QuestStructure questStructure in quest.AllItemsNeeded)
            {
                questStructure.IsCompleted = false;
                questStructure.CurrentAmount = 0;
            }
        }

        AllMissionsCompleted = false;
    }

    private bool AllItemsCompleted()
    {
        var currentQuest = GameObject.Find(LevelManager.GetCurrentLevel()?.currentQuest)?.GetComponent<quest>();
        if (currentQuest == null)
        {
            return false;
        }
        foreach (QuestStructure questItem in currentQuest.AllItemsNeeded)
        {
            if (!questItem.IsCompleted)
            {
                return false; // If any item is not completed, return false
            }
        }
        return true; // All items are completed
    }

    public bool IsAllQuestsInMapDone() {
        var allQuestsDone = true;
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Quest"))
        {
            quest quest = gameObject.GetComponent<quest>();
            if (quest != null)
            {
                allQuestsDone = allQuestsDone && quest.AllMissionsCompleted;
            }
        }
        return allQuestsDone;
    }
}
