using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

public enum LevelEnum
{
    NONE,
    LEVEL_1,
    LEVEL_2,
    LEVEL_3,
    MENU
}

[Serializable]
public class AudioLevel
{
    public LevelEnum level;
    public AudioSource audioSource;
}

public class Level
{
    public LevelEnum level;
    public List<String> questsGameObject = new List<String>();
    public string currentQuest = "";
    public bool isFinished = false;
}

public class LevelManager : MonoBehaviour
{
    public static LevelEnum currentLevel = LevelEnum.NONE;
    public List<AudioLevel> audioLevels = new List<AudioLevel>();

    public List<Level> levels = new List<Level>();
    // Start is called before the first frame update

    public bool isCurrentLevelFinished = false;

    public void DefineLevels()
    {
        // Define levels
        var level1 = new Level();
        level1.level = LevelEnum.LEVEL_1;
        level1.questsGameObject.AddRange(new string[] { "StartQuestMachine_Quest1", "StartQuestMachine_Quest2" });
        level1.currentQuest = "StartQuestMachine_Quest1";
        levels.Add(level1);
    }
    void Start()
    {
        DefineLevels();
        
        setLevel(LevelEnum.LEVEL_1);
        setAudioLevel(LevelEnum.LEVEL_1);
        change_skybox.Change_Skybox(2);

        StartCoroutine(Config.Waiter(() => { }, () =>
        {
            var welcomeMachine = GameObject.Find("WelcomeMachine");
            welcomeMachine.GetComponentInChildren<TextMachine>().StartDialog();
        }, 1f));
    }

    // Update is called once per frame
    void Update()
    {  
        Debug.Log("Current level" + currentLevel);
        updateLevelObjects();
        CheckLevelQuests();
    }

    public static void setLevel(LevelEnum level, bool withoutAudioChange = false)
    {
        currentLevel = level;
    }

    public static Level GetCurrentLevel()
    {
        var levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        return levelManager.levels.Find(level => level.level == currentLevel);
    }

    private void resetLevelObjects()
    {
        var doorToLevel2 = GameObject.Find("door_to_level_2");
        doorToLevel2.GetComponent<DoorController>().isLocked = false;
    }

    private void updateLevelObjects()
    {
        resetLevelObjects();
        switch (currentLevel)
        {
            case LevelEnum.LEVEL_1:
                // get door_to_level_2 and prevent that door to open
                GameObject.Find("door_to_level_2").GetComponent<DoorController>().isLocked = isCurrentLevelFinished ? false : true;
                break;
            case LevelEnum.LEVEL_2:
                // get door_to_level_2 and prevent that door to open
                GameObject.Find("door_to_level_2").GetComponent<DoorController>().isLocked = true;
                break;
            case LevelEnum.LEVEL_3:
                break;
            case LevelEnum.MENU:
                break;
        }
    }

    public static void setAudioLevel(LevelEnum level)
    {
        var audioLevels = GameObject.Find("LevelManager").GetComponent<LevelManager>().audioLevels;
        foreach (AudioLevel audioLevel in audioLevels)
        {
            if (audioLevel.level == level)
            {
                audioLevel.audioSource.Play();
            }
            else
            {
                audioLevel.audioSource.Stop();
            }
        }
    }

    public static void StartQuestLevel()
    {
        var levelStructure = GetCurrentLevel(); 
        switch (levelStructure.level)
        {
            case LevelEnum.LEVEL_1:
                // start mission on Gameobject with quest script
                
                // get level
                var questToStart = levelStructure.currentQuest;
                var questGameObject = GameObject.Find(questToStart)?.GetComponent<quest>();
                if (questGameObject != null)
                {
                    questGameObject.StartQuest();
                }
        
                break;
        }
    }

    private void CheckLevelQuests()
    {
        if (StateManager.isDialogRunning && questionmanager.CurrentQuest != null)
        {
            return;
        }

        var currentLevelStructure = GetCurrentLevel();
        // check if all quests from current level are completed
        var allQuestionsAreCompletedFromCurrentLevel = false;

        if (currentLevelStructure != null)
        {
            allQuestionsAreCompletedFromCurrentLevel = currentLevelStructure.questsGameObject.TrueForAll(questGameObject =>
            {
                var quest = GameObject.Find(questGameObject)?.GetComponent<quest>();
                return quest != null && quest.Isfinished;
            });
        }

        Debug.Log("test - " + allQuestionsAreCompletedFromCurrentLevel + " - " + levels.Find(level => level.level == currentLevel).questsGameObject.Count);

        if (!allQuestionsAreCompletedFromCurrentLevel)
        {
            // Check if current quest is completed. If so, start the next one if exists
            var currentLevel = GetCurrentLevel();
            var currentQuestGameObject = GameObject.Find(currentLevel.currentQuest);
            if (currentQuestGameObject != null)
            {
                var currentQuest = currentQuestGameObject.GetComponent<quest>();
                if (currentQuest.Isfinished)
                {
                    var allQuestsInLevel = currentLevel.questsGameObject;
                    var currentQuestIndex = allQuestsInLevel.IndexOf(currentLevel.currentQuest);
                    
                    if (currentQuestIndex == allQuestsInLevel.Count - 1)
                    {
                        // last quest from level
                        allQuestionsAreCompletedFromCurrentLevel = true;
                        currentLevel.currentQuest = "";
                    }
                    else
                    {
                        // next quest
                        currentLevel.currentQuest = allQuestsInLevel[currentQuestIndex + 1];

                        // Update quest level object
                        foreach (var quest in Resources.FindObjectsOfTypeAll<quest>())
                        {
                            var gameObject = quest.gameObject;
                            switch (gameObject.name)
                            {
                                case "StartQuestMachine_Quest2":
                                    GameObject.Find("StartQuestMachine_Quest1")?.SetActive(false);
                                    gameObject?.SetActive(true);
                                    
                                    var globalDialogMachine = GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>();
                                    globalDialogMachine.machineSentences = new string[] { 
                                        "Congratulations, you completed the first quest! :)",
                                        "Now, let's start the second quest. You must learn how to play the minigames",
                                        "To learn more about the quest, please interact with the ATM on your left :)",
                                    };
                                    globalDialogMachine.shouldStartDialogProgrammatically = true;  
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        if (allQuestionsAreCompletedFromCurrentLevel && !isCurrentLevelFinished)
        {
            switch (currentLevel)
            {
                case LevelEnum.LEVEL_1:
                    var globalDialogMachine = GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>();
                    globalDialogMachine.machineSentences = new string[] { "Congratulations, you completed all questions of level 1. The maze door is been unlocked. Go check outside for level two :)" };
                    globalDialogMachine.shouldStartDialogProgrammatically = true;
                    isCurrentLevelFinished = true;
                    break;
                case LevelEnum.LEVEL_2:
                    setLevel(LevelEnum.LEVEL_3);
                    break;
            }

            levels.Find(level => level.level == currentLevel).isFinished = true;
        }
    }
}
