using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMachine : MonoBehaviour
{
    public Canvas canvas = null;
    public GameObject objectInteractor = null;

    private bool enteredCollider = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
       {
        if (!objectInteractor) {
            return;
        }

        canvas.enabled = enteredCollider;
        if (enteredCollider && Input.GetKeyDown("r")) {
            //Destroy(objectInteractor);
            StateManager.chatCanvasShouldRender = true;
            StateManager.sentencesDialog.Add("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce in mauris nec tortor iaculis luctus sed vitae turpis. Etiam ut dignissim enim. Pellentesque auctor leo id viverra dictum.");
            StateManager.sentencesDialog.Add("Testing sentence");
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (objectInteractor) {
            canvas.enabled = true;
        }
        enteredCollider = true;
        Debug.Log("test collider");
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        canvas.enabled = false;
        enteredCollider = false;
        StateManager.chatCanvasShouldRender = false;
    }
}
