using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera Camera;
    public List<GameObject> PuzzlePieces;
    public static GameObject currentPiece;

    private static Vector3[] PuzzlePiecesOriginalPosition;
    private static List<GameObject> _PuzzlePieces = new List<GameObject>();

    void Start()
    {
        PuzzlePiecesOriginalPosition = new Vector3[PuzzlePieces.Count];
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            _PuzzlePieces.Add(PuzzlePieces[i]);
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
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                    PuzzlePiece.GetComponent<MeshRenderer>().material.SetColor("_Color", PuzzlePiece == currentPiece ? Color.blue : Color.white);
                }
            }

            // Get canvas with DialogType.PUZZLE and set validate button to interactive true if state manager current puzzle anwsers has 4 elements
            StateManager.SelectedDialogCanvas.Canvas.gameObject.GetComponentInChildren<UnityEngine.UI.Button>().interactable = StateManager.CurrentPuzzleAnswers.Count == 4;
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
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
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

    public static void resetElement(GameObject gameObject)
    {
        for (var i = 0; i < _PuzzlePieces.Count; i++)
        {
            Debug.Log("testing element " + gameObject.name + " " + _PuzzlePieces[i].name);
            if (gameObject.name == _PuzzlePieces[i].name)
            {
                _PuzzlePieces[i].transform.position = PuzzlePiecesOriginalPosition[i];
                break;
            }
        }
    }
}
