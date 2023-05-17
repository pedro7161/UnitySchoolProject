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
    public bool PlayerEnteredOnCollider = false;

   
    void Start()
    {
        if (AllItemsNeeded != null)
        {
            questionmanager.AllItemsNeeded = AllItemsNeeded;
        }
    }

    void FixedUpdate()
    {
        
        if (PlayerEnteredOnCollider && Input.GetKey(KeyCode.R))
        {
            Debug.Log("Quest name: " + Quest_name);
            Debug.Log("Quest description: " + Quest_description);
            Debug.Log("Quest code: " + Quest_code);
            Debug.Log("Quest dificulty: " + dificulty);
            Debug.Log("Quest is finished: " + Isfinished);
            Debug.Log("Quest items needed: " + questionmanager.AllItemsNeeded.Count);
            Debug.Log("Current quest: " + questionmanager.CurrentQuest);

                Debug.Log("cliquei no R");
                if (questionmanager.CurrentQuest == null)
                {
                    Debug.Log("Quest is going to start");
                    questionmanager.MissionStart(this);
                }
                else if (questionmanager.CurrentQuest.Isfinished && questionmanager.CurrentQuest != null)
                {
                    Debug.Log("Quest is going to be finished/closed for good");
                   
                    questionmanager.MissionEnd();
                }
                else if (!questionmanager.CurrentQuest.Isfinished && questionmanager.CurrentQuest != null)
                {
                    // Display a message or take appropriate action to indicate that a quest is already in progress
                    Debug.Log("A quest is already in progress");
                }
            
        }
        else if (questionmanager.HasAllItems && PlayerEnteredOnCollider)
                {
                 
                    Debug.Log("make quest finisheble");
                   questionmanager.MissionCompleted();
                }      
        else{
            Debug.Log("Player is not on collider");}
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerEnteredOnCollider = true;
        Debug.Log("Player on Collider?" + PlayerEnteredOnCollider);
        Debug.Log("Pressed R key:" + Input.GetKey(KeyCode.R));

    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player on Collider?" + PlayerEnteredOnCollider);
        PlayerEnteredOnCollider = false;
    }

}

