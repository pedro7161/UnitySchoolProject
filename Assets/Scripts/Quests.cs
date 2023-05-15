using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
   public int questId;  // The quest ID associated with the NPC's quest

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(questId);
            questmanager.instance.StartQuest(questId);
        }
    }
}
