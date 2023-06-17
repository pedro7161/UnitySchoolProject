using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        Debug.Log("Puzzle piece clicked " + GetComponentInParent<Puzzle>().name + " " + PuzzleStartCollider.puzzleToExecute);
        if (GetComponentInParent<Puzzle>().name != PuzzleStartCollider.puzzleToExecute)
        {
            return;
        }

        if (StateManager.SelectedMinigame == MinigameType.PUZZLE)
        {
            Puzzle.currentPiece = gameObject;
        }
    }
}
