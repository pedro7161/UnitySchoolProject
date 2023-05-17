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

   void Start() {
        if (AllItemsNeeded != null)
        {
            questionmanager.AllItemsNeeded = AllItemsNeeded;
        }
   }
   private void OnTriggerEnter(Collider other) {
         if (other.gameObject.tag == "Player")
         {
             if (Input.GetKeyDown("R") && questionmanager.CurrentQuest == null)
                {
                   questionmanager.MissionStart(this);
                
                }
                if(Input.GetKeyDown("R") && questionmanager.CurrentQuest.Isfinished == true && questionmanager.CurrentQuest != null)
                {
                    questionmanager.MissionEnd();
                }
                else{
                    // avisar que ja tem uma quest em andamento
                    return;
                }
                
         }
   }
}
