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

    public bool invertDirections = false;
    public float speed = 0.5f;

    void Start()
    {
        _PuzzlePieces = PuzzlePieces;
        PuzzlePiecesOriginalPosition = new Vector3[PuzzlePieces.Count];
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
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

            var canvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.PUZZLE);
            if (canvas == null)
            {
                return;
            }

            var button = canvas?.Canvas?.gameObject?.GetComponentInChildren<UnityEngine.UI.Button>();
            if (button) {
                // Get canvas with DialogType.PUZZLE and set validate button to interactive true if state manager current puzzle anwsers has 4 elements
                canvas.Canvas.gameObject.GetComponentInChildren<UnityEngine.UI.Button>().interactable = StateManager.CurrentPuzzleAnswers.Count == 4;
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
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            PuzzlePieces[i].transform.position = PuzzlePiecesOriginalPosition[i];
        }
    }

    void handlePieceMovement()
    {
        var speedNormalized = speed * Time.deltaTime;

        if (invertDirections)
        {
            //wasd keyboard movement
            if (Input.GetKey(KeyCode.W))
            {
                currentPiece.transform.position += new Vector3(-speedNormalized, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                currentPiece.transform.position += new Vector3(speedNormalized, 0, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                currentPiece.transform.position += new Vector3(0, 0, -speedNormalized);
            }
            if (Input.GetKey(KeyCode.D))
            {
                currentPiece.transform.position += new Vector3(0, 0, speedNormalized);
            }
        }
        else
        {
            //wasd keyboard movement
            if (Input.GetKey(KeyCode.W))
            {
                currentPiece.transform.position += new Vector3(0, 0, -speedNormalized);
            }
            if (Input.GetKey(KeyCode.S))
            {
                currentPiece.transform.position += new Vector3(0, 0, speedNormalized);
            }
            if (Input.GetKey(KeyCode.A))
            {
                currentPiece.transform.position += new Vector3(speedNormalized, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                currentPiece.transform.position += new Vector3(-speedNormalized, 0, 0);
            } 
        }
    }

    public static void resetElement(GameObject gameObject)
    {
        var index = _PuzzlePieces.IndexOf(gameObject);
        gameObject.transform.position = PuzzlePiecesOriginalPosition[index];
    }
}
