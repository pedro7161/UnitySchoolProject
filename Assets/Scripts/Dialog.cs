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

        // Start is called before the first frame update
        // Update is called once per frame
        void Update()
        {
            if (StateManager.SelectedDialogCanvas == null)
            {
                return;
            }

            if (StateManager.chatCanvasShouldRender && !StateManager.isDialogRunning)
            {
                StartDialog();
            }

            if (!StateManager.chatCanvasShouldRender && StateManager.isDialogRunning)
            {
                StopDialog();
            }

            if (StateManager.chatCanvasShouldRender && StateManager.isDialogRunning)
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
                }
            }
        }

        public void ConfigureExtraSteps() {
            // Update canvas properties
            switch (StateManager.SelectedDialogCanvas.DialogType) {
                case DialogType.CODE_CHALLENGE:
                    StateManager.SelectedDialogCanvas.CanvasTitle.text = StateManager.SelectedQuestion.title;

                    for (int i = 0; i < StateManager.SelectedQuestion.answers.Count; i++)
                    {
                        StateManager.SelectedDialogCanvas.Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = StateManager.SelectedQuestion.answers[i];
                    }

                    break;
                case DialogType.ALERT:
                    Debug.Log(StateManager.AnswerFromSelectedQuestion);
                    StateManager.SelectedDialogCanvas.Canvas.GetComponentInChildren<Image>().color = StateManager.AnswerFromSelectedQuestion == "Correct" ? 
                        new Color(103f/255f, 149f/255f, 107f/255f, 100f/255f) : new Color(178f/255f, 123f/255f, 110f/255f, 100f/255f);
                        
                        // Alert dialog should close automatically
                        StartCoroutine(Config.Waiter(() => {}, () => {
                            StopDialog();
                            // Can't use OnStopDialog because we need to keep the answer after interaction until new setup dialog
                            StateManager.AnswerFromSelectedQuestion = null;
                        }, 1));
                    break;
            }
        }

        private void CompleteCurrentSentence()
        {
            StopCoroutineHandler();
            isTextBeingTyped = false;

            if (!getDisplay() || StateManager.sentencesDialog.Count == 0)
            {
                return;
            }
            getDisplay().text = StateManager.sentencesDialog[currentIndex];
        }

        private void StartDialog()
        {
            if (StateManager.SelectedDialogCanvas.DialogType == DialogType.DIALOG && StateManager.sentencesDialog.Count == 0)
            {
                return;
            }

            ConfigureExtraSteps();
            getCanvas().gameObject.SetActive(true);
            StateManager.OnStartDialog();
            currentIndex = 0;

            if (StateManager.useAnimationOnDialog) 
            {
                dialogCoroutine = StartCoroutine(UpdateDisplay());
            } else 
            {
                CompleteCurrentSentence();
            }
        }

        public void StopDialog()
        {
            getCanvas().gameObject.SetActive(false);
            StopCoroutineHandler();

            if (getDisplay())
            {
                getDisplay().text = string.Empty;
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

        private TextMeshProUGUI getDisplay() => StateManager.SelectedDialogCanvas?.OutputSentences;
        private Canvas getCanvas() => StateManager.SelectedDialogCanvas?.Canvas;

        private IEnumerator UpdateDisplay()
        {
            var display = getDisplay();
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
            var index = int.Parse(gameObject.name);

            var text = StateManager.AnswerFromSelectedQuestion = StateManager.SelectedQuestion.correct_answer == index ? "Correct" : "Incorrect";
            StopDialog();
            StateManager.SetupDialog(new List<string>{text}, DialogType.ALERT, false);
        }
    }
}