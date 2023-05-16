using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera Camera;
    public GameObject[] PuzzlePieces;
    private Vector3[] PuzzlePiecesOriginalPosition;
    public static GameObject currentPiece;

    void Start() {
        PuzzlePiecesOriginalPosition = new Vector3[PuzzlePieces.Length];
        for (int i = 0; i < PuzzlePieces.Length; i++) {
            PuzzlePiecesOriginalPosition[i] = PuzzlePieces[i].transform.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Camera.enabled = StateManager.SelectedMinigame == MinigameType.PUZZLE;
        if (StateManager.SelectedMinigame == MinigameType.PUZZLE)
        {
            if (currentPiece) 
            {
                handlePieceMovement();
                foreach (GameObject PuzzlePiece in PuzzlePieces) 
                {
                    Rigidbody rigidbody = PuzzlePiece.GetComponentInChildren<Rigidbody>();
                    rigidbody.useGravity = true;
                    rigidbody.isKinematic = false;
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    if (PuzzlePiece == currentPiece) {
                        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    } else {
                        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    PuzzlePiece.GetComponent<MeshRenderer>().material.SetColor("_Color", PuzzlePiece == currentPiece ? Color.blue : Color.white);
                }
            }
        }
        else
        {
            resetPiecesPosition();
            if (currentPiece) 
            {
                currentPiece.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
                currentPiece = null;
            }
        }
    }

    void resetPiecesPosition() 
    {
        for (int i = 0; i < PuzzlePieces.Length; i++) {
            PuzzlePieces[i].transform.position = PuzzlePiecesOriginalPosition[i];
        }
    }

    void handlePieceMovement() 
    {
        var speed = 2.5f * Time.deltaTime;
        //wasd keyboard movement
        if (Input.GetKey(KeyCode.W))
        {
            currentPiece.transform.position += new Vector3(0, 0, -speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentPiece.transform.position += new Vector3(0, 0, speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            currentPiece.transform.position += new Vector3(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentPiece.transform.position += new Vector3(-speed, 0, 0);
        }
    }
}
