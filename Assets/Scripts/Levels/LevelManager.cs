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
    MENU,
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

public enum SoundEffectEnum
{
    ITEM_COLLECT,
    QUEST_COMPLETE
}

[Serializable]
public class SoundEffect
{
    public SoundEffectEnum soundEffect;
    public AudioSource audioSource;
}

public class LevelManager : MonoBehaviour
{
    public static LevelEnum currentLevel = LevelEnum.NONE;
    public List<AudioLevel> audioLevels = new List<AudioLevel>();
    public List<SoundEffect> soundEffects = new List<SoundEffect>();

    public List<Level> levels = new List<Level>();
    // Start is called before the first frame update

    public static bool isCurrentLevelFinished = false;
    public static bool isAnyQuestRunning = false;
    public static bool shouldShowWelcomeMessage = true;

    public void DefineLevels()
    {
        // Define levels
        var level1 = new Level();
        level1.level = LevelEnum.LEVEL_1;
        level1.questsGameObject.AddRange(new string[] { "StartQuestMachine_Quest1", "StartQuestMachine_Quest2" });
        level1.currentQuest = "StartQuestMachine_Quest1";

        levels.Add(level1);

        var level2 = new Level();
        level2.level = LevelEnum.LEVEL_2;
        level2.questsGameObject.AddRange(new string[] { "QuestMachine_Quest3", "QuestMachine_Quest4" });
        level2.currentQuest = "QuestMachine_Quest3";
        levels.Add(level2);

        var level3 = new Level();
        level3.level = LevelEnum.LEVEL_3;
        level3.questsGameObject.AddRange(new string[] { "QuestMachine_Quest5", "QuestMachine_Quest6" });
        level3.currentQuest = "QuestMachine_Quest5";
        levels.Add(level3);
    }
    void Start()
    {
        DefineLevels();
        
        setLevel(LevelEnum.LEVEL_1);
    }

    // Update is called once per frame
    void Update()
    {  
        if (shouldShowWelcomeMessage)
        {
            StartCoroutine(Config.Waiter(() => { }, () =>
            {
                var welcomeMachine = GameObject.Find("WelcomeMachine");
                if (welcomeMachine == null)
                {
                    return;
                }
                welcomeMachine.GetComponentInChildren<TextMachine>().StartDialog();
            }, 1.5f));
        }
        Debug.Log("Current level = " + currentLevel);
        updateLevelObjects();
        CheckLevelQuests();
    }

    public static void setLevel(LevelEnum level, bool withoutAudioChange = false)
    {
        switch(level)
        {
            case LevelEnum.LEVEL_1:
                setAudioLevel(LevelEnum.LEVEL_1);
                change_skybox.Change_Skybox(2);
                break;
            case LevelEnum.LEVEL_2:
                setAudioLevel(LevelEnum.LEVEL_2);
                change_skybox.Change_Skybox(0);
                break;
            case LevelEnum.LEVEL_3:
                setAudioLevel(LevelEnum.LEVEL_3);
                change_skybox.Change_Skybox(1);
                break;
            
        }
        Debug.Log("Setting level to " + level);
        currentLevel = level;
        isCurrentLevelFinished = false;
    }

    public static Level GetCurrentLevel()
    {
        var levelManager = GameObject.Find("LevelManager")?.GetComponent<LevelManager>();
        if (levelManager == null)
        {
            return null;
        }
        return levelManager.levels.Find(level => level.level == currentLevel);
    }

    private void resetLevelObjects()
    {
        var doorToLevel2 = GameObject.Find("door_to_level_2");
        if (doorToLevel2 != null)
        {
            doorToLevel2.GetComponent<DoorController>().isLocked = false;
        }
    }

    private void updateLevelObjects()
    {
        var doorLevel2 = GameObject.Find("door_to_level_2");
        if (doorLevel2 == null)
        {
            return;
        }
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
        var questToStart = levelStructure.currentQuest;
        var questGameObject = GameObject.Find(questToStart)?.GetComponent<quest>();
        if (questGameObject != null)
        {
            questGameObject.StartQuest();
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
        var allQuestionsAreCompletedFromCurrentLevel = true;

        var questsStructure = Resources.FindObjectsOfTypeAll<quest>().ToList().Where(x => currentLevelStructure.questsGameObject.Contains(x.gameObject.name));
        foreach (quest quest in questsStructure)
        {
            allQuestionsAreCompletedFromCurrentLevel = allQuestionsAreCompletedFromCurrentLevel && quest.Isfinished;
        }

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
                        var questsFilteredByCurrentLevel = Resources.FindObjectsOfTypeAll<quest>().ToList().Where(x => currentLevel.questsGameObject.Contains(x.gameObject.name));
                        foreach (var quest in questsFilteredByCurrentLevel)
                        {
                            var globalDialogMachine = GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>();
                            var gameObject = quest.gameObject;
                            switch (gameObject.name)
                            {
                                case "StartQuestMachine_Quest2":
                                    GameObject.Find("StartQuestMachine_Quest1")?.SetActive(false);
                                    gameObject?.SetActive(true);
                                    
                                    globalDialogMachine.machineSentences = new string[] { 
                                        "Congratulations, you completed the first quest! :)",
                                        "Now, let's start the second quest. You must learn how to play the minigames",
                                        "To learn more about the quest, please interact with the ATM on your left :)",
                                    };
                                    globalDialogMachine.shouldStartDialogProgrammatically = true;
                                break;
                                case "QuestMachine_Quest4":
                                    GameObject.Find("QuestMachine_Quest3")?.SetActive(false);
                                    gameObject?.SetActive(true);
                                    
                                    globalDialogMachine.machineSentences = new string[] { 
                                        "Congratulations, you completed another quest! :)",
                                        "To start another quest, please interact again with the terminal :)",
                                    };
                                    globalDialogMachine.shouldStartDialogProgrammatically = true;
                                    questionmanager.CurrentQuest = null;
                                break;
                                case "QuestMachine_Quest6":
                                    GameObject.Find("QuestMachine_Quest5")?.SetActive(false);
                                    gameObject?.SetActive(true);
                                    
                                    globalDialogMachine.machineSentences = new string[] { 
                                        "Congratulations, you completed another quest! :)",
                                        "To start another quest, please interact again with the terminal :)",
                                    };
                                    globalDialogMachine.shouldStartDialogProgrammatically = true;
                                    questionmanager.CurrentQuest = null;
                                break;

                            }
                        }
                    }
                }
            }
        }

        Debug.Log("allQuestionsAreCompletedFromCurrentLevel = " + allQuestionsAreCompletedFromCurrentLevel + " isCurrentLevelFinished = " + isCurrentLevelFinished + " StateManager.SelectedDialogCanvas.Count = " + StateManager.SelectedDialogCanvas.Count);
        
        if (allQuestionsAreCompletedFromCurrentLevel && !isCurrentLevelFinished && StateManager.SelectedDialogCanvas.Count == 0)
        {
            var globalDialogMachine = GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>();
            Debug.Log("test - should finish the current level");
            switch (currentLevel)
            {
                case LevelEnum.LEVEL_1:
                    globalDialogMachine.machineSentences = new string[] { "Congratulations, you completed all questions of level 1. The maze door is been unlocked. Go check outside for level two :)" };
                    globalDialogMachine.shouldStartDialogProgrammatically = true;
                    isCurrentLevelFinished = true;
                    questionmanager.CurrentQuest = null;
                    break;
                case LevelEnum.LEVEL_2:
                    globalDialogMachine.machineSentences = new string[] { "Congratulations, you completed all questions of level 2. You may want to go forward to go to the level 3. It's a sunset environment :)", "Please cross the whole street and interact with the orange machine to start the last level" };
                    globalDialogMachine.StartDialog();
                    isCurrentLevelFinished = true;
                    break;
                case LevelEnum.LEVEL_3:
                    globalDialogMachine.machineSentences = new string[] { "Congratulations, you completed all questions of level 3.", "Thank you for playing our game :) You are now free to explore to your hearts desire the entire map." };
                    globalDialogMachine.StartDialog();
                    isCurrentLevelFinished = true;
                    GameObject.Find("door_to_level_2").GetComponent<DoorController>().isLocked = false;
                    break;
            }

            levels.Find(level => level.level == currentLevel).isFinished = true;
        }
    }
}
