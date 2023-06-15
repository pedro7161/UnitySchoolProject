using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas warningCanvas;

    public static string zoneName = string.Empty;
    private static LevelEnum currentZone = LevelEnum.LEVEL_1;
    public LevelEnum zoneToUpdate = LevelEnum.LEVEL_1;
    public static bool shouldShowLevelTwoMessage = true;
    public static bool shouldShowCannotGoToMessage = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void UpdateEnvironment()
    {
        var gameObjectName = this.gameObject.name;
        switch (gameObjectName)
        {
            case "Zone1":
                if (LevelManager.GetCurrentLevel().level != LevelEnum.LEVEL_1)
                {
                    return;
                }
                LevelManager.setAudioLevel(LevelEnum.LEVEL_1);
                change_skybox.Change_Skybox(2);
                break;
            case "Zone2":
                LevelManager.setAudioLevel(LevelEnum.LEVEL_2);
                change_skybox.Change_Skybox(0);
                break;
            case "Zone3":
                LevelManager.setAudioLevel(LevelEnum.LEVEL_3);
                change_skybox.Change_Skybox(1);
                break;
        }
    }
    
    private void UpdateLevel()
    {
        Debug.Log("Current zone " + currentZone);
        Debug.Log("Current zone to update " + zoneToUpdate);
        switch (zoneToUpdate)
        {
            case LevelEnum.LEVEL_2:
                if (currentZone == LevelEnum.LEVEL_1)
                {
                    LevelManager.setLevel(LevelEnum.LEVEL_2);
                    if (shouldShowLevelTwoMessage)
                    {
                        var globalDialogMachine = GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>();
                        globalDialogMachine.machineSentences = new string[] { "Welcome to open world. The level 2 will start soon. Please interact with the yellow machine to start the quests" };
                        globalDialogMachine.shouldStartDialogProgrammatically = true;
                        shouldShowLevelTwoMessage = false;
                    }
                }
                break;
            case LevelEnum.LEVEL_3:
                if (currentZone == LevelEnum.LEVEL_2)
                {
                    LevelManager.setLevel(LevelEnum.LEVEL_3);
                }
                break;
        }
    }

    public void ConfigureCollider()
    {
        var colliders = GetComponents<BoxCollider>().ToList();
        var level = LevelManager.GetCurrentLevel();
        switch (gameObject.name)
        {
            case "next_area_3":
                if (level.level == LevelEnum.LEVEL_2 && level.isFinished)
                {
                    colliders.Find(collider => !collider.isTrigger).enabled = false;
                } 
                else if (shouldShowCannotGoToMessage)
                {
                    var globalDialogMachine = GameObject.Find("GlobalDialogMachine").GetComponentInChildren<TextMachine>();
                    globalDialogMachine.machineSentences = new string[] { "You cannot go to the next area. Please finish the current level quests first" };
                    globalDialogMachine.StartDialog();
                    shouldShowCannotGoToMessage = false;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "Environment")
        {
            if (zoneName != gameObject.name)
            {
                zoneName = gameObject.name;
                UpdateEnvironment();
                //warningCanvas.enabled = true;
            }
        }
        
        if (other.gameObject.tag == "Player" && gameObject.tag == "Zone")
        {
            ConfigureCollider();
            UpdateLevel();
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        shouldShowCannotGoToMessage = true;
    }
}
