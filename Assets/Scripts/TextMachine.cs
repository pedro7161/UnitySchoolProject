using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMachine : MonoBehaviour
{
    private Canvas canvas = new Canvas();
    private GameObject atmBlue = null;

    private bool enteredCollider = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("textCanvas").GetComponent<Canvas>();
        atmBlue = GameObject.Find("ATM_Blue");
    }

    // Update is called once per frame
    void Update()
       {
        if (atmBlue && enteredCollider && Input.GetKeyDown("r")) {
            //Destroy(atmBlue);
            canvas.enabled = false;
            StateManager.chatCanvasShouldRender = true;
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (atmBlue) {
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
