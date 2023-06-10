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
    public static  List<DialogCanvasStructure> SelectedDialogCanvas = new List<DialogCanvasStructure>();
    public static bool useAnimationOnDialog = false;
    // Questions information
    public static Question SelectedQuestion = null;
    public static string LastAnswerFromSelectedQuestion = null;

    // Minigame information
    public static MinigameType SelectedMinigame = MinigameType.NONE;

    // Puzzle information
    public static QuestionPuzzle SelectedQuestionPuzzle = null;
    public static Dictionary<string, string> CurrentPuzzleAnswers = new Dictionary<string, string>();
    public static string LastAnswerFromSelectedPuzzleQuestion = null;

    // Typescripting Information
    public static bool LastResultFromTypingscript = false;

    public static void SetDialogCanvasData(HashSet<DialogCanvasStructure> data) {
        StateManager.dialogCanvasData = data;
    }

    public static void SetupDialog(List<string> sentences, DialogType dialogType, bool useAnimationOnDialog = false, string objectInteractor = null) {
        StateManager.sentencesDialog.Clear();
        StateManager.sentencesDialog.AddRange(sentences);

        var canvas = StateManager.dialogCanvasData.First(canvas => canvas.DialogType == dialogType);
        if (canvas != null && !StateManager.SelectedDialogCanvas.Contains(canvas))
        {
            StateManager.SelectedDialogCanvas.Add(canvas);
        }

        StateManager.chatCanvasShouldRender = true;
        StateManager.useAnimationOnDialog = useAnimationOnDialog;

        // Set minigame if dialogType as any value that match with a minigame enumerator
        if (Enum.IsDefined(typeof(MinigameType), dialogType.ToString()) && DialogType.QUEST != dialogType)
        {
            StateManager.SelectedMinigame = (MinigameType) Enum.Parse(typeof(MinigameType), dialogType.ToString());
        }
    }

    public static void OnStartDialog() 
    {
        StateManager.isDialogRunning = true;
    }

    public static void OnStopDialog() 
    {
        StateManager.chatCanvasShouldRender = false;
        StateManager.isDialogRunning = false;
        var canvasToBeRemoved = new List<DialogCanvasStructure>();

        // remove all active dialog canvas from selected dialog canvas
        StateManager.SelectedDialogCanvas.ForEach(canvas => {
            if (
                (canvas.DialogType == DialogType.QUEST && !LevelManager.GetCurrentLevel().isFinished) || 
                (canvas.DialogType != DialogType.QUEST && Enum.IsDefined(typeof(MinigameType), canvas.DialogType.ToString()) && canvas.Canvas.gameObject.activeSelf))
            {
                return;
            }
            canvasToBeRemoved.Add(canvas);
        });


        canvasToBeRemoved.ForEach(canvas => {
            canvas.Canvas.gameObject.SetActive(false);
            StateManager.SelectedDialogCanvas.Remove(canvas);
        });
        
        //StateManager.SelectedDialogCanvas = null;
        StateManager.SelectedQuestion = null;
        StateManager.SelectedQuestionPuzzle = null;
        StateManager.CurrentPuzzleAnswers = new Dictionary<string, string>();
    }
}
