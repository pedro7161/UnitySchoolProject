using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItems : MonoBehaviour
{
    public questionmanager questionmanager; // Instance of the questionmanager class

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision detected");
        // Debug.Log("questionmanager.CurrentQuest: " + questionmanager.CurrentQuest);
        // Debug.Log("questionmanager.CurrentQuest.AllItemsNeeded != null: " + (questionmanager.CurrentQuest.AllItemsNeeded != null));
        // Debug.Log("questionmanager.CurrentQuest != null: " + (questionmanager.CurrentQuest != null));

        if ((questionmanager.CurrentQuest != null) && (questionmanager.CurrentQuest.AllItemsNeeded != null))
        {
            foreach (QuestStructure questItem in questionmanager.CurrentQuest.AllItemsNeeded)
            {
                if (questItem.Item.Contains(this.gameObject))
                {
                    questItem.CurrentAmount++;
                    questionmanager.UpdateQuest();
                    gameObject.SetActive(false);
                    if (questItem.CurrentAmount >= questItem.AmountRequired)
                    {
                        questItem.IsCompleted = true;
                        // Call the quest manager's method to handle the completion of this item
                        questionmanager.UpdateQuest();
                    }
                }
            }

        }
    }
    public void ActivateGameObject()
    {
        gameObject.SetActive(true);
        // Additional logic or actions to be performed when the game object is activated
    }
}
