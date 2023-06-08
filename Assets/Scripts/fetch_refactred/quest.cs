using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class QuestStructure
{
    public List<GameObject> Item;
    public int CurrentAmount;
    public int AmountRequired;
    public bool IsCompleted;
    public string ItemName;
    public bool isMinigameQuest = false;
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

    private void Start()
    {
        if (AllItemsNeeded != null)
        {
            questionmanager.AllItemsNeeded = AllItemsNeeded;
        }
    }

    private void Update()
    {
        if (StateManager.isDialogRunning && StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.ALERT) != null)
        {
            return;
        }

        if (PlayerEnteredOnCollider && Input.GetKey(KeyCode.R) || shouldStartQuestProgramatically)
        {
            if (questionmanager.CurrentQuest == null && !AllMissionsCompleted)
            {
                shouldStartQuestProgramatically = false;
                questionmanager.MissionStart(this);
                StateManager.SetupDialog(new List<string>(), DialogType.QUEST, false);
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
                    StateManager.SetupDialog(new List<string>{"Quest Completed!"}, DialogType.ALERT, false);
                }
                else
                {
                    AllMissionsCompleted = true;
                    questionmanager.MissionEnd();
                }

            }
            else if (!questionmanager.CurrentQuest.Isfinished && questionmanager.CurrentQuest != null)
            {
                // Display a message or take appropriate action to indicate that a quest is already in progress
            }
            else if (questionmanager.CurrentQuest.Isfinished)
            {
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
        foreach (QuestStructure questItem in AllItemsNeeded)
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
