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

    public bool AllQuestsCompleted = false;

    private void Start()
    {
        if (AllItemsNeeded != null)
        {
            questionmanager.AllItemsNeeded = AllItemsNeeded;
        }
    }

    private void Update()
    {
        if (StateManager.isDialogRunning && StateManager.SelectedDialogCanvas?.DialogType == DialogType.ALERT)
        {
            return;
        }

        if (PlayerEnteredOnCollider && Input.GetKey(KeyCode.R))
        {
            StateManager.SelectedMinigame = MinigameType.FETCH_QUEST;

            Debug.Log("Clicked R");
            if (questionmanager.CurrentQuest == null && !AllQuestsCompleted)
            {
                Debug.Log("Quest is going to start");
                questionmanager.MissionStart(this);
                StateManager.SetupDialog(new List<string>(), DialogType.FETCH_QUEST, false);
            }

            else if (AllItemsCompleted())
            {
                if (AllQuestsCompleted)
                {
                    Debug.Log("All quests are completed");
                    StateManager.chatCanvasShouldRender = false;
                    StateManager.OnStopDialog();

                    var questCanvas = GameObject.Find("QuestCanvas");
                    if (questCanvas != null)
                    {
                        questCanvas.SetActive(false);
                    }
                    StateManager.SetupDialog(new List<string>{"Quest Completed!"}, DialogType.ALERT, false);
                    
                    if (IsAllQuestsInMapDone()) 
                    {
                        ResetAllQuestsInMap();
                    }
                    return;
                }
                else
                {
                    Debug.Log("Quest is going to be finished/closed for good");
                    AllQuestsCompleted = true;
                    questionmanager.MissionEnd();
                }

            }
            else if (!questionmanager.CurrentQuest.Isfinished && questionmanager.CurrentQuest != null)
            {
                // Display a message or take appropriate action to indicate that a quest is already in progress
                Debug.Log("A quest is already in progress");
            }
            else if (questionmanager.CurrentQuest.Isfinished)
            {
                Debug.Log("Quest is already been done/closed");

            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }
        else if (questionmanager.HasAllItems && PlayerEnteredOnCollider)
        {
            Debug.Log("Make quest finishable");
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
        Debug.Log("Player on Collider? " + PlayerEnteredOnCollider);
        Debug.Log("Pressed R key: " + Input.GetKey(KeyCode.R));

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player on Collider? " + PlayerEnteredOnCollider);
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

        AllQuestsCompleted = false;
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
                Debug.Log("Quest: " + quest.Quest_name + " is completed? " + quest.AllQuestsCompleted);
                allQuestsDone = allQuestsDone && quest.AllQuestsCompleted;
            }
        }

        return allQuestsDone;
    }
}
