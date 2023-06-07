using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isLocked;
    // Start is called before the first frame update
    void Start()
    {
        isLocked = false;
        var animator = GetComponent<Animator>();
        animator.SetBool("character_nearby", false);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshCollider>().convex = isLocked;
    }
     void OnTriggerEnter(Collider other) {
        if (isLocked) {
            return;
        }
        var animator = GetComponent<Animator>();
        animator.SetBool("character_nearby", true);
        
    }
    void OnTriggerExit(Collider other) {
        var animator = GetComponent<Animator>();
        animator.SetBool("character_nearby", false);
    }
}

