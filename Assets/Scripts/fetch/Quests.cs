using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Quests : MonoBehaviour
{
   public int questId;  // The quest ID associated with the NPC's quest
   public int requiredItemCount; 
   public TextMeshProUGUI canvasText;
   public Canvas canvas;

   public bool questcompleted = false;
     // Flag to indicate if the quest has been completed
    private void Start()
    {
        canvas.enabled = false;
        canvasText = canvas.GetComponentInChildren<TextMeshProUGUI>();

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (inventory.instance.HasItem(questId) && other.CompareTag("Player") && !questcompleted)
        {
                canvas.enabled = true;

               
                canvasText.text = "Mission Complete";
                questcompleted = true;
                questmanager.instance.CompleteQuest(questId);
                Debug.Log("Quest completed: " + questId);
              
         
        }
        if (other.CompareTag("Player") && !questcompleted)
        {
            canvas.enabled = true;
            canvasText.text = "Mission Incomplete";
        }
        if (other.CompareTag("Player") && questcompleted)
        {
            canvas.enabled = true;
            canvasText.text = "Mission Complete";
        }    
        else {
            Debug.Log("Quest started: " + questId);
                questmanager.instance.StartQuest(questId,requiredItemCount);

            }
        }

   
}
