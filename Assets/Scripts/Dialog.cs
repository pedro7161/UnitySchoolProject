using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace StarterAssets
{
    public class Dialog : MonoBehaviour
    {
        private int currentIndex = 0;
        private Coroutine dialogCoroutine = null;
        public float speedText = 0.005f;
        private bool isTextBeingTyped = false;

        private List<DialogCanvasStructure> dialogCanvasStructuresInProgress = new List<DialogCanvasStructure>();

        // Start is called before the first frame update
        // Update is called once per frame
        void Update()
        {
            // Syncronize the dialogCanvasStructuresInProgress with the SelectedDialogCanvas by removing old elements on dialogCanvasStructuresInProgress
            dialogCanvasStructuresInProgress.RemoveAll(
                dialogCanvas => StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == dialogCanvas.DialogType) == null);
            Debug.Log("DialogCanvasStructuresInProgress: " + dialogCanvasStructuresInProgress.Count + " SelectedDialogCanvas: " + StateManager.SelectedDialogCanvas.Count);

            var dialogCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG);
            if (StateManager.SelectedDialogCanvas.Count == 0)
            {
                return;
            }

            if (StateManager.chatCanvasShouldRender && (!StateManager.isDialogRunning || questionmanager.CurrentQuest != null))
            {
                StartDialog();
            }

            if (!StateManager.chatCanvasShouldRender && StateManager.isDialogRunning)
            {
                StopDialog();
            }

            if (
                StateManager.chatCanvasShouldRender && StateManager.isDialogRunning &&
                StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG) != null
            )
            {
                HandlePlayerInput();
            }
        }

        private void HandlePlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTextBeingTyped)
                {
                    CompleteCurrentSentence();
                    return;
                }

                if (currentIndex < StateManager.sentencesDialog.Count - 1)
                {
                    currentIndex++;
                    StopCoroutineHandler();
                    dialogCoroutine = StartCoroutine(UpdateDisplay());
                }
                else
                {
                    StopDialog();
                    GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>().shouldStartDialogProgrammatically = false;
                }
            }
        }

        public void ConfigureExtraSteps() {
            if (StateManager.SelectedDialogCanvas.Count == 0)
            {
                return;
            }

            StateManager.SelectedDialogCanvas.ForEach(canvas => {
                // Update canvas properties
                switch (canvas.DialogType) {
                    case DialogType.DIALOG:
                        if (StateManager.useAnimationOnDialog) 
                        {
                            dialogCoroutine = StartCoroutine(UpdateDisplay());
                        } else 
                        {
                            CompleteCurrentSentence();
                        }
                        break;
                    case DialogType.CODE_CHALLENGE:
                        canvas.CanvasTitle.text = StateManager.SelectedQuestion.title;
                        canvas.OutputSentences.text = StateManager.SelectedQuestion.code;

                        for (int i = 0; i < StateManager.SelectedQuestion.answers.Count; i++)
                        {
                            canvas.Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = StateManager.SelectedQuestion.answers[i];
                        }

                        break;
                    case DialogType.ALERT:
                        // On dialog mode, we should only one sentence
                        canvas.OutputSentences.text = StateManager.sentencesDialog[0];
                        var anwser = string.Empty;
                        var isCorrectBool = false;

                        switch(StateManager.SelectedMinigame)
                        {
                            case MinigameType.CODE_CHALLENGE:
                                anwser = StateManager.LastAnswerFromSelectedQuestion;
                                break;
                            case MinigameType.PUZZLE:
                                anwser = StateManager.LastAnswerFromSelectedPuzzleQuestion;
                                break;
                            case MinigameType.TYPE_SCRIPTING_CHALLENGE:
                                isCorrectBool = StateManager.LastResultFromTypingscript;
                                break;
                        }

                        if (questionmanager.CurrentQuest != null && questionmanager.CurrentQuest.Isfinished || LevelManager.GetCurrentLevel().isFinished)
                        {
                            isCorrectBool = true;
                        }
                        
                        Debug.Log("Selected Minigame value on alert lol: " + StateManager.SelectedMinigame);
                        if (StateManager.SelectedMinigame != MinigameType.NONE)
                        {
                            StateManager.SelectedMinigame = MinigameType.NONE;
                        }

                        canvas.Canvas.GetComponentInChildren<Image>().color =
                                anwser == "Correct" || isCorrectBool ?
                                    new Color(103f / 255f, 149f / 255f, 107f / 255f, 100f / 255f) :
                                    new Color(178f / 255f, 123f / 255f, 110f / 255f, 100f / 255f);

                        // Alert dialog should close automatically
                        StartCoroutine(Config.Waiter(() => {}, () => {
                            StopDialog();
                        }, 1));

                        break;
                    case DialogType.PUZZLE:
                        canvas.OutputSentences.text = StateManager.SelectedQuestionPuzzle.code;
                        break;
                }
            });
        }

        private void CompleteCurrentSentence()
        {
            StopCoroutineHandler();
            isTextBeingTyped = false;
            
            var display = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG)?.OutputSentences;

            if (!display || StateManager.sentencesDialog.Count == 0)
            {
                return;
            }
            display.text = StateManager.sentencesDialog[currentIndex];
        }

        private void StartDialog()
        {
            if (
                StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG) != null &&
                StateManager.sentencesDialog.Count == 0)
            {
                return;
            }

            if (StateManager.SelectedDialogCanvas.Count == dialogCanvasStructuresInProgress.Count)
            {
                return;
            }

            // Check if a dialog on selectedialogcanvas exists on dialogCanvasStructuresInProgres
            Debug.Log(StateManager.SelectedDialogCanvas.Count);
            Debug.Log(dialogCanvasStructuresInProgress.Count);

            ConfigureExtraSteps();
            StateManager.SelectedDialogCanvas.ForEach(canvas => canvas.Canvas.gameObject.SetActive(true));
            StateManager.OnStartDialog();
            currentIndex = 0;
            Debug.Log("Reseting current index to 0");

            // Update the dialogCanvasStructuresInProgress with elements from SelectedDialogCanvas that are not present on dialogCanvasStructuresInProgress. Filter by DialogType
            StateManager.SelectedDialogCanvas.ForEach(canvas => {
                if (dialogCanvasStructuresInProgress.Find(dialogCanvas => dialogCanvas.DialogType == canvas.DialogType) == null)
                {
                    dialogCanvasStructuresInProgress.Add(canvas);
                }
            });
        }

        public void StopDialog()
        {
           
            currentIndex = 0;

            StateManager.SelectedDialogCanvas.ForEach(canvas => {
                if (canvas.DialogType != DialogType.QUEST)
                {
                    canvas.Canvas.gameObject.SetActive(false);
                }
            });
            var displayDialog = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG)?.OutputSentences;
            StopCoroutineHandler();

            if (displayDialog != null)
            {
                displayDialog.text = string.Empty;
            }
            // It's importante to set OnStopDialog after all activities beause OnStopDialog sets the SeletecedDialogCanvas to null
            StateManager.OnStopDialog();
        }

        private void StopCoroutineHandler()
        {
            if (dialogCoroutine != null)
            {
                StopCoroutine(dialogCoroutine);
            }
        }

        // private TextMeshProUGUI getDisplay() => StateManager.SelectedDialogCanvas?.OutputSentences;
        // private Canvas getCanvas() => StateManager.SelectedDialogCanvas?.Canvas;

        private IEnumerator UpdateDisplay()
        {
            // use on Alert and dialog
            var display = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG || canvas.DialogType == DialogType.ALERT)?.OutputSentences;
            if (!display || StateManager.sentencesDialog.Count == 0) {
                yield break;
            }

            display.text = string.Empty;
            isTextBeingTyped = true;
            foreach (char c in StateManager.sentencesDialog[currentIndex].ToCharArray())
            {
                display.text += c;
                yield return new WaitForSeconds(speedText);
            }
            isTextBeingTyped = false;
        }

        public void OnDialogButtonsHandler(GameObject gameObject) {
            var answer = "";
            
            StateManager.SelectedDialogCanvas.ForEach(canvas => {
                switch (canvas.DialogType) {
                    case DialogType.CODE_CHALLENGE:
                        var index = int.Parse(gameObject.name);
                        answer = StateManager.LastAnswerFromSelectedQuestion = StateManager.SelectedQuestion.correct_answer == index ? "Correct" : "Incorrect";

                        break;
                    case DialogType.PUZZLE:
                        var isCorrect = StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.capsule == StateManager.CurrentPuzzleAnswers["capsule"] &&
                            StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.cube == StateManager.CurrentPuzzleAnswers["cube"] &&
                            StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.sphere == StateManager.CurrentPuzzleAnswers["sphere"] &&
                            StateManager.SelectedQuestionPuzzle.PuzzleQuestionsOrder.cylinder == StateManager.CurrentPuzzleAnswers["cylinder"];

                            StateManager.LastAnswerFromSelectedPuzzleQuestion = isCorrect ? "Correct" : "Incorrect";
                            answer = StateManager.LastAnswerFromSelectedPuzzleQuestion;
                            var status = GameObject.Find("PuzzleChallengeStatus").GetComponentInChildren<TextMeshProUGUI>();

                            if (!isCorrect) 
                            {
                                status.text = "The combination is invalid! Try again.";
                                status.color = Color.red;
                                return;
                            }
                            
                            status.text = "";
                            status.color = Color.white;
                        break;
                }
            });


            StopDialog();
            StateManager.SetupDialog(new List<string>{answer}, DialogType.ALERT, false);
        }
    }
}