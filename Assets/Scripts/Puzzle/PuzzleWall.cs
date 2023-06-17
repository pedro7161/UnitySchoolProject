using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) {
            var instance = GameObject.Find(PuzzleStartCollider.puzzleToExecute).GetComponents<Puzzle>();
            Debug.Log("Puzzle to execute: " + PuzzleStartCollider.puzzleToExecute + " gameObject name: " + gameObject.name);
            foreach(var Puzzle in instance)
            {
                Debug.Log("Resetting puzzle: " + Puzzle.name);
                Puzzle.resetElement(other.gameObject);
            }
        }  
    }
}
