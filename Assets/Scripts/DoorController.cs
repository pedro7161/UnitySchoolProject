using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("character_nearby", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnTriggerEnter(Collider other) {
        Debug.Log("test collider");
        var animator = GetComponent<Animator>();
        animator.SetBool("character_nearby", true);
        
    }
    void OnTriggerExit(Collider other) {
        var animator = GetComponent<Animator>();
        animator.SetBool("character_nearby", false);
    }
}

