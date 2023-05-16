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
     // Flag to indicate if the quest has been completed
    private void start()
    {
        canvas.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (inventory.instance.HasItem(questId) && other.CompareTag("Player"))
        {
                canvas.enabled = true;
                canvasText = GetComponent<TextMeshProUGUI>();
                canvasText.text = "Mission Complete";
                questmanager.instance.CompleteQuest(questId);
              
         
        }
        else {
            Debug.Log("Quest started: " + questId);
                questmanager.instance.StartQuest(questId,requiredItemCount);
            }
        }

   
}
