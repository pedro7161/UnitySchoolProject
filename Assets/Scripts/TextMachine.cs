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

    // Update is called once per frame
    void Update()
    {

        if ((enteredCollider && Input.GetKeyDown("r") || shouldStartDialogProgrammatically) && !StateManager.isDialogRunning && StateManager.SelectedMinigame == MinigameType.NONE)
        {
            ConfigureCanvas();
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
        StateManager.SetupDialog(sentences, ActionCanvasType, ActionCanvasType != DialogType.CODE_CHALLENGE, gameObject.transform.parent.gameObject.name);
        
        shouldStartDialogProgrammatically = false;
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
