using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
    public int questId;  // The quest ID associated with this triggerable object

    private bool isTriggerable = false;  // Flag to indicate if the object is currently triggerable

    private void Start()
    {
        // Subscribe to the event for quest start
        questmanager.instance.OnQuestStarted += CheckTriggerability;
    }

    private void CheckTriggerability(int startedQuestId)
    {
        if (startedQuestId == questId)
        {
            isTriggerable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerable && other.CompareTag("Player"))
        {
            // Perform actions when the player collides with this object during the active quest
        }
    }
}
