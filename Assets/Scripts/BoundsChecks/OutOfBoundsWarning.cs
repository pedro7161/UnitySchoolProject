using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsWarning : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas warningCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
            
            if (other.gameObject.tag == "Player") {
                Debug.Log("Player is out of bounds");
                warningCanvas.enabled = true;

            }
    }
    private void OnTriggerExit(Collider other) {
            
            if (other.gameObject.tag == "Player") {
                Debug.Log("Player is in bounds");
                warningCanvas.enabled = false;

            }
    }
}
