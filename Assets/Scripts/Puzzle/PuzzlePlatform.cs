using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzlePlatform : MonoBehaviour
{
    private bool isPlatformOcupied = false;
    private int objectsOnPlatform = 0;
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
        objectsOnPlatform++;
        // validate only those with layer PuzzleCollider
        if (other.gameObject.layer == 3 && !isPlatformOcupied) {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            var key = other.gameObject.name.ToLower();
            isPlatformOcupied = true;
            
            StateManager.CurrentPuzzleAnswers[key] = gameObject.name;
        }
    }

    void OnTriggerExit(Collider other)
    {
        objectsOnPlatform--;
        if (other.gameObject.layer == 3 && isPlatformOcupied && objectsOnPlatform == 0)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            var key = other.gameObject.name.ToLower();
            StateManager.CurrentPuzzleAnswers.Remove(key);
            isPlatformOcupied = false;
        }
    }
}
