using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool chatCanvasShouldRender = false;
    public static bool isDialogRunning = false;
    public static List<string> sentencesDialog = new List<string>();
    
    // Dialogs information
    private static HashSet<DialogCanvasStructure> dialogCanvasData = new HashSet<DialogCanvasStructure>();
    public static  DialogCanvasStructure SelectedDialogCanvas = null;
    public static bool useAnimationOnDialog = false;
    // Questions information
    public static Question SelectedQuestion = null;
    public static string AnswerFromSelectedQuestion = null;

    public static void SetDialogCanvasData(HashSet<DialogCanvasStructure> data) {
        StateManager.dialogCanvasData = data;
    }

    public static void SetupDialog(List<string> sentences, DialogType dialogType, bool useAnimationOnDialog = false) {
        StateManager.sentencesDialog.Clear();
        StateManager.sentencesDialog.AddRange(sentences);
        
        StateManager.SelectedDialogCanvas = StateManager.dialogCanvasData.First(canvas => canvas.DialogType == dialogType);

        StateManager.chatCanvasShouldRender = true;
        StateManager.useAnimationOnDialog = useAnimationOnDialog;
    }

    public static void OnStartDialog() 
    {
        StateManager.isDialogRunning = true;
    }

    public static void OnStopDialog() 
    {
        StateManager.chatCanvasShouldRender = false;
        StateManager.isDialogRunning = false;
        
        StateManager.SelectedDialogCanvas = null;
        StateManager.SelectedQuestion = null;
    }
}
