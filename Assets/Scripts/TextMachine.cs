using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMachine : MonoBehaviour
{
    public Canvas canvas = null;
    public DialogType ActionCanvasType;
    private bool enteredCollider = false;
    public string[] machineSentences;
    // Start is called before the first frame update

    public GameObject externalObjectToActivateOnInteract = null;

    public bool shouldStartDialogProgrammatically = false;
    void Update()
    {
        Debug.Log("Configuring canvas..." + shouldStartDialogProgrammatically);
        if (
            (enteredCollider && Input.GetKeyDown("r") || shouldStartDialogProgrammatically) && (
            !StateManager.isDialogRunning || questionmanager.CurrentQuest != null) && 
            StateManager.SelectedMinigame == MinigameType.NONE &&
            StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == ActionCanvasType) == null)
        {
            if (ShouldConfigureCanvas())
            {
                ConfigureCanvas();
            }
        } 
    }

    public void StartDialog()
    {
        ConfigureCanvas();
    }

    public bool ShouldConfigureCanvas()
    {
        switch(ActionCanvasType)
        {
            case DialogType.PUZZLE:
                var isQuestPuzzle = true;

                if (questionmanager.CurrentQuest != null && !isQuestPuzzle)
                {
                    return false;
                }
                return true;
            default:
                return true;
        }
    }

    public void ConfigureCanvas()
    {
        List<string> sentences = new List<string>();
        if (ActionCanvasType == DialogType.CODE_CHALLENGE)
        {
            Question question = Config.GetRandomQuestion();
            StateManager.SelectedQuestion = question;
            
            sentences.Add(question.code);
        } 
        else
        {
            sentences.AddRange(machineSentences);
        }

        Debug.Log("Current dialog Type: " + ActionCanvasType);
        //shouldStartDialogProgrammatically = false;
        StateManager.SetupDialog(sentences, ActionCanvasType, ActionCanvasType != DialogType.CODE_CHALLENGE, gameObject.transform.parent.gameObject.name);
        if (externalObjectToActivateOnInteract != null)
        {
            externalObjectToActivateOnInteract.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        enteredCollider = true;
    }

    void OnTriggerExit(Collider other)
    {
        enteredCollider = false;
        if (StateManager.isDialogRunning || StateManager.SelectedMinigame != MinigameType.NONE)
        {
            return;
        }
        StateManager.OnStopDialog();
    }
}
