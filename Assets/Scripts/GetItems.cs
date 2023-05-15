using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
public class GetItems : MonoBehaviour
{ 

    public Image imageComponent; // Assign the Image component in the Inspector
    public Sprite newSprite; // Assign the new sprite in the Inspector
    public int questId;  // The quest ID associated with this triggerable object

    private bool isTriggerable = false;  // Flag to indicate if the object is currently triggerable

    private void Start()
    {
        // Subscribe to the event for quest start
        questmanager.instance.OnQuestStarted += CheckTriggerability;
        //  Debug.Log("Event subscription executed");
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
            imageComponent.sprite = newSprite;
            Destroy(this.gameObject);
            // Perform actions when the player collides with this object during the active quest
        }
    }
}
   
