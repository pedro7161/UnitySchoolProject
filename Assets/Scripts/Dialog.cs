using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StarterAssets
{
    public class Dialog : MonoBehaviour
    {
        private int currentIndex = 0;
        private TextMeshProUGUI display = null;
        private Coroutine dialogCoroutine = null;
        public Canvas canvas = null;
        public float speedText = 0.005f;
        public bool isTextBeingTyped = false;
        
        // Start is called before the first frame update
        // Update is called once per frame
        void Update()
        {
            if (StateManager.chatCanvasShouldRender && !StateManager.isDialogRunning) {
                StartDialog();
            }

            if (!StateManager.chatCanvasShouldRender && StateManager.isDialogRunning) {
                Debug.Log("should stop coroutine update display");
                StopDialog();
            }

            if (StateManager.chatCanvasShouldRender && StateManager.isDialogRunning) {
                HandlePlayerInput();
            }
        }

        void HandlePlayerInput() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (isTextBeingTyped) {
                    CompleteCurrentSentence();
                    return;
                }

                if (currentIndex < StateManager.sentencesDialog.Count - 1) {
                    currentIndex++;
                    StopCoroutine(dialogCoroutine);
                    dialogCoroutine = StartCoroutine(UpdateDisplay());

                    // Clear sentencesDialog list after finished all the sentences
                    StateManager.sentencesDialog.Clear();
                } else {
                    StopDialog();
                }
            }
        }

        void CompleteCurrentSentence() {
            StopCoroutine(dialogCoroutine);
            getDisplay().text = StateManager.sentencesDialog[currentIndex];
            isTextBeingTyped = false;
        }

        void StartDialog() {
            if (StateManager.sentencesDialog.Count == 0) {
                return;
            }
            
            canvas.gameObject.SetActive(true);
            StateManager.isDialogRunning = true;
            currentIndex = 0;

            dialogCoroutine = StartCoroutine(UpdateDisplay());
        }

        void StopDialog() {
            canvas.gameObject.SetActive(false);
            StateManager.isDialogRunning = false;
            StateManager.chatCanvasShouldRender = false;

            getDisplay().text = string.Empty;
            StopCoroutine(dialogCoroutine);
        }

        private TextMeshProUGUI getDisplay() {
            if (!display) {
                display = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
            }

            return display;
        }

        private IEnumerator UpdateDisplay()
        {
            getDisplay().text = string.Empty;
            isTextBeingTyped = true;
            foreach(char c in StateManager.sentencesDialog[currentIndex].ToCharArray()) {
                getDisplay().text += c;
                yield return new WaitForSeconds(speedText);
            }
            isTextBeingTyped = false;
        }
    }
}