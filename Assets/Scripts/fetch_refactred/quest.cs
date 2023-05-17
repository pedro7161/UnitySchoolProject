using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class QuestStructure
{

    public itemsquest Item = new itemsquest();

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

    void Start()
    {
        if (AllItemsNeeded != null)
        {
            questionmanager.AllItemsNeeded = AllItemsNeeded;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger funciona");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("sou um player tag");
            Debug.Log("Quest name: " + Quest_name + questionmanager.CurrentQuest);
            Debug.Log("Current quest: "+ questionmanager.CurrentQuest);

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (questionmanager.CurrentQuest is null)
                {
                    Debug.Log("Quest is going to start");
                    questionmanager.MissionStart(this);
                }
                else if (questionmanager.CurrentQuest.Isfinished && questionmanager.CurrentQuest != null)
                {
                    Debug.Log("Quest is going to be finished");
                    questionmanager.MissionEnd();
                }
                else
                {
                    // Display a message or take appropriate action to indicate that a quest is already in progress
                    Debug.Log("A quest is already in progress");
                }
            }
        }
    }

}

