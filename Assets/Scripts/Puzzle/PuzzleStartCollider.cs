using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStartCollider : MonoBehaviour
{
    public bool PlayerEnteredOnCollider = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerEnteredOnCollider && Input.GetKey(KeyCode.R) && StateManager.SelectedMinigame == MinigameType.NONE) {
            StateManager.SelectedMinigame = MinigameType.PUZZLE;
            StateManager.SelectedQuestionPuzzle = Config.GetRandomQuestionPuzzle();

            Debug.Log("Selected question puzzle anwsers");
            Debug.Log("sphere " + StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.sphere);
            Debug.Log("capsule " + StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.capsule);
            Debug.Log("cube " + StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.cube);
            Debug.Log("cylinder " + StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.cylinder);

            StateManager.SetupDialog(new List<string>{StateManager.SelectedQuestionPuzzle.code}, DialogType.PUZZLE, false);
        }

        if (StateManager.SelectedMinigame == MinigameType.PUZZLE && PlayerEnteredOnCollider && Input.GetKey(KeyCode.Escape)) {
            StateManager.SelectedMinigame = MinigameType.NONE;
            StateManager.chatCanvasShouldRender = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerEnteredOnCollider = true;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerEnteredOnCollider = false;
    }
}
