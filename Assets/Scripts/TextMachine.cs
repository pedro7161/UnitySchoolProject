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

    public bool shouldStartDialogProgrammatically = false;
    void Update()
    {
        if (
            (enteredCollider && Input.GetKeyDown("r") || shouldStartDialogProgrammatically) && 
            (!StateManager.isDialogRunning || questionmanager.CurrentQuest != null) && 
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
            Question question;
            if (LevelManager.GetCurrentLevel().level == LevelEnum.LEVEL_1)
            {
                question = Config.GetQuestions().Find(x => x.difficulty == QuestionDifficulty.EASY);
            }
            else
            {
                question = Config.GetRandomQuestion();
            }
            StateManager.SelectedQuestion = question;
            
            sentences.Add(question.code);
        } 
        else
        {
            sentences.AddRange(machineSentences);
            switch(this.gameObject.transform.parent.gameObject.name)
            {
                case "StartQuestMachine_Dialog":
                    var levelInstance = GameObject.Find("LevelManager").GetComponent<LevelManager>();
                    var levelStructure = levelInstance.levels.Find(x => x.level == LevelEnum.LEVEL_1);

                    if (levelStructure != null)
                    {
                        if (levelStructure.isFinished)
                        {   
                            sentences.Clear();
                            sentences.Add("Congratulations, you completed all questions of level 1. The maze door is been unlocked. Go check outside for level two :)");
                        }
                        switch (levelStructure.currentQuest)
                        {
                            case "StartQuestMachine_Quest2":
                                sentences.Clear();
                                sentences.AddRange(
                                    new string[] { 
                                            "Congratulations, you completed the first quest! :)",
                                            "Now, let's start the second quest. You must learn how to play the minigames",
                                            "We have four types of minigames: Typescripting challenge, Puzzle challenge, Code challenge and...",
                                            "... what you already experienced, the item fetch challenge.",
                                            "...",
                                            "Typescript challenge -> You must write some programming terms with a timer running. If you don't write the terms in time, you lose.",
                                            "...",
                                            "Puzzle challenge -> You have four objects to place in the correct position. You need to discover the correct position by ...",
                                            "... reading the code and understanding what the code does",
                                            "...",
                                            "Code challenge -> You must read the question and choose the correct answer. If you choose the wrong answer, you lose.",
                                            "To start the quest, please press R when interacting with the terminal on my right.",
                                            "When you complete the quest, please interact with the terminal to complete the mission!"
                                        }
                                );
                                break;
                        }
                    }
                break;
            }
        }

        //shouldStartDialogProgrammatically = false;
        StateManager.SetupDialog(sentences, ActionCanvasType, ActionCanvasType != DialogType.CODE_CHALLENGE, gameObject.transform.parent.gameObject.name);
        //UpdateGameObjectQuests();
        UpdateGameObjectQuests();
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

    void UpdateGameObjectQuests() {
        var allQuestsInMap = Resources.FindObjectsOfTypeAll<quest>();

        var currentLevel = LevelManager.GetCurrentLevel();
        switch (currentLevel.level)
        {
            case LevelEnum.LEVEL_1:
                
                foreach(var resource in allQuestsInMap)
                {
                    var gameObject = resource.gameObject;
                    
                    switch (gameObject.name)
                    {
                        case "StartQuestMachine_Quest1":
                            if (
                                currentLevel.currentQuest == "StartQuestMachine_Quest1" && 
                                (this.gameObject.transform.parent.name == "StartQuestMachine_Dialog" && enteredCollider) // this quest object appers if player interact for the first time with the machine
                            )
                            {
                                gameObject.SetActive(true);
                            }
                            break;
                    }
                }
                break;
        }
    }
}
