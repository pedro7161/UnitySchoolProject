using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItems : MonoBehaviour
{
    public Image imageComponent; // Assign the Image component in the Inspector
    public Sprite newSprite; // Assign the new sprite in the Inspector
    public int questId; // The quest ID associated with this triggerable object

    private HashSet<int> collectedItems = new HashSet<int>(); // Track the collected items

    private bool isTriggerable = false; // Flag to indicate if the object is currently triggerable

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
            Debug.Log("Quest started: " + questId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerable && other.CompareTag("Player"))
        {
            if (!collectedItems.Contains(questId))
            {
                imageComponent.sprite = newSprite;
                inventory.instance.AddItem(questId);
                collectedItems.Add(questId);
                // Perform actions when the player collides with this object during the active quest
            }
            else
            {
                // Display a message or perform any other action indicating that the item has already been obtained
                Debug.Log("Item already collected: " + questId);
            }

            // Check if the item is still needed
            bool itemStillNeeded = questmanager.instance.IsItemStillNeeded(questId);
            if (!itemStillNeeded)
            {
                Destroy(gameObject); // Destroy the game object if the item is no longer needed
            }
        }
    }
}
