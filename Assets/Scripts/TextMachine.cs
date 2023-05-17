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

        Debug.Log("Current object name: " + gameObject.name);
        Debug.Log("Current dialog Type: " + ActionCanvasType);
        StateManager.SetupDialog(sentences, ActionCanvasType, ActionCanvasType != DialogType.CODE_CHALLENGE);
    }

    void OnTriggerEnter(Collider other)
    {
        enteredCollider = true;
    }

    void OnTriggerExit(Collider other)
    {
        enteredCollider = false;
        StateManager.OnStopDialog();
    }
}
