using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzlePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        // validate only those with layer PuzzleCollider
        if (other.gameObject.layer == 3) {
            var key = other.gameObject.name.ToLower();
            
            StateManager.CurrentPuzzleAnswers[key] = gameObject.name;
            Debug.Log("Add element: " + "key: " + key + " value: " + gameObject.name);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 3) {
            var key = other.gameObject.name.ToLower();
            StateManager.CurrentPuzzleAnswers.Remove(key);
            Debug.Log("Remove element: " + "key: " + key + " value: " + gameObject.name);
        }
    }
}
