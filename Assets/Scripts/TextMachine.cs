using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMachine : MonoBehaviour
{
    public Canvas canvas = null;
    public GameObject objectInteractor = null;
    public DialogType ActionCanvasType = DialogType.DIALOG;

    private bool enteredCollider = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!objectInteractor)
        {
            return;
        }

        canvas.enabled = enteredCollider;
        if (enteredCollider && Input.GetKeyDown("r") && !StateManager.isDialogRunning)
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
            sentences.AddRange(Config.templateSentences);
        }

        StateManager.SetupDialog(sentences, ActionCanvasType, ActionCanvasType != DialogType.CODE_CHALLENGE);
    }

    void OnTriggerEnter(Collider other)
    {
        canvas.enabled = enteredCollider = true;
    }

    void OnTriggerExit(Collider other)
    {
        canvas.enabled = enteredCollider = false;
        StateManager.OnStopDialog();
    }
}
