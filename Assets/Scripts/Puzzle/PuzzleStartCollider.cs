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
        if (
            PlayerEnteredOnCollider && Input.GetKey(KeyCode.R) && StateManager.SelectedMinigame == MinigameType.NONE &&
            (StateManager.SelectedDialogCanvas.Count == 0 || StateManager.SelectedDialogCanvas.Count == 1 && StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.QUEST) != null)	
        ) {
            List<QuestionPuzzle> questions = new List<QuestionPuzzle>();

            switch (LevelManager.GetCurrentLevel().level)
            {
                case LevelEnum.LEVEL_1:
                    questions.AddRange(Config.GetPuzzleQuestions().FindAll(x => x.Difficulty == QuestionDifficulty.EASY));
                    break;
                case LevelEnum.LEVEL_2:
                    questions.AddRange(Config.GetPuzzleQuestions().FindAll(x => x.Difficulty == QuestionDifficulty.MEDIUM));
                    break;
            }

            if (questions.Count > 0)
                StateManager.SelectedQuestionPuzzle = questions[Random.Range(0, questions.Count)];
            else
                StateManager.SelectedQuestionPuzzle = Config.GetRandomQuestionPuzzle();

            StateManager.SetupDialog(new List<string>{StateManager.SelectedQuestionPuzzle.code}, DialogType.PUZZLE, false);
        }

        if (StateManager.SelectedMinigame == MinigameType.PUZZLE && Input.GetKey(KeyCode.Escape)) {
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
