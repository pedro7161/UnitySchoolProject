using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMachine : MonoBehaviour
{
    public Canvas canvas = null;
    public DialogType ActionCanvasType;
    public bool enteredCollider = false;
    public string[] machineSentences;
    // Start is called before the first frame update

    public static string textMachineToExecute = string.Empty;

    public bool shouldStartDialogProgrammatically = false;
    void Update()
    {
        if (
            (enteredCollider && Input.GetKeyDown("r") && TypeScriptingManager.canInteractWithMachine|| shouldStartDialogProgrammatically) && 
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
        if (StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.DIALOG) == null)
        {
            ConfigureCanvas();
        }
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
            Question question = null;
            var levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            var currentLevel = LevelManager.GetCurrentLevel().level;
            if (levelManager.levels.Find(level => !level.isFinished) != null)
            {
                switch (currentLevel)
                {
                    case LevelEnum.LEVEL_1:
                        question = Config.GetRandomQuestion(QuestionDifficulty.EASY);
                        break;
                    case LevelEnum.LEVEL_2:
                        question = Config.GetRandomQuestion(QuestionDifficulty.MEDIUM);
                        break;
                    case LevelEnum.LEVEL_3:
                        question = Config.GetRandomQuestion(QuestionDifficulty.HARD);
                        break;
                    default:
                        question = Config.GetRandomQuestion();
                        break;
                }
            }
            
            if (question == null)
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
                    var levelStructure = LevelManager.GetCurrentLevel();

                    if (levelStructure != null)
                    {
                        if (levelStructure.isFinished)
                        {   
                            sentences.Clear();
                            // normalize LEVEL_1 to Level 1
                            var levelName = levelStructure.level.ToString().Replace("_", " ").ToLower();

                            sentences.Add($"Congratulations, you completed all questions of {levelName}. The maze door is been unlocked. Go check outside for level two :)");
                            questionmanager.CurrentQuest = null;
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
                                questionmanager.CurrentQuest = null;
                                break;
                        }
                    }
                break;
            }
        }

        shouldStartDialogProgrammatically = false;
        Debug.Log("Setting up dialog with action canvas type: " + ActionCanvasType);
        StateManager.SetupDialog(sentences, ActionCanvasType, ActionCanvasType != DialogType.CODE_CHALLENGE, gameObject.transform.parent.gameObject.name);
        //UpdateGameObjectQuests();
        UpdateGameObjectQuests();
    }

    void OnTriggerEnter(Collider other)
    {
        enteredCollider = true;
        textMachineToExecute = this.gameObject.transform.parent.gameObject.name;
    }

    void OnTriggerExit(Collider other)
    {
        enteredCollider = false;
        textMachineToExecute = string.Empty;
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
            case LevelEnum.LEVEL_2:
                foreach(var resource in allQuestsInMap)
                {
                    var gameObject = resource.gameObject;
                    
                    switch (gameObject.name)
                    {
                        case "QuestMachine_Quest3":
                            if (
                                currentLevel.currentQuest == "QuestMachine_Quest3" && 
                                (this.gameObject.transform.parent.name == "OpenWorldStartMachine" && enteredCollider) // this quest object appers if player interact for the first time with the machine
                            )
                            {
                                gameObject.SetActive(true);
                            }
                            break;
                    }
                }
                break;
            case LevelEnum.LEVEL_3:
                foreach(var resource in allQuestsInMap)
                {
                    var gameObject = resource.gameObject;
                    
                    switch (gameObject.name)
                    {
                        case "QuestMachine_Quest5":
                            if (
                                currentLevel.currentQuest == "QuestMachine_Quest5" && 
                                (this.gameObject.transform.parent.name == "OpenWorldStartMachineLevel3" && enteredCollider) // this quest object appers if player interact for the first time with the machine
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
