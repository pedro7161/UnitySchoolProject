using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas warningCanvas;
    public LevelEnum currentZone = LevelEnum.LEVEL_1;
    public LevelEnum previousZone = LevelEnum.LEVEL_1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateLevelMusic()
    {
        var gameObjectName = this.gameObject.name;
        switch (gameObjectName)
        {
            case "next_area_1":
                LevelManager.setLevel(LevelEnum.LEVEL_1);
                break;
            case "next_area_2":
                LevelManager.setLevel(LevelEnum.LEVEL_2);
                break;
            case "next_area_3":
                LevelManager.setLevel(LevelEnum.LEVEL_3);
                break;
        }
    }
    private void UpdateEnvironment()
    {
        var gameObjectName = this.gameObject.name;
        switch (gameObjectName)
        {
            case "next_area_1":
                LevelManager.setAudioLevel(LevelEnum.LEVEL_1);
                change_skybox.Change_Skybox(2);
                break;
            case "next_area_2":
                currentZone = LevelEnum.LEVEL_2;
                
                LevelManager.setAudioLevel(LevelEnum.LEVEL_2);
                change_skybox.Change_Skybox(0);
                break;
            case "next_area_3":
                currentZone = LevelEnum.LEVEL_3;
             
                LevelManager.setAudioLevel(LevelEnum.LEVEL_3);
                change_skybox.Change_Skybox(1);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            updateLevelMusic();
            UpdateEnvironment();
            //warningCanvas.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(this.gameObject.name == "next_area_2")
            {
                previousZone = currentZone = LevelEnum.LEVEL_2;
            }
            else{
            previousZone = currentZone;
            }
            updateLevelMusic();    
            UpdateEnvironment();
            //warningCanvas.enabled = false;
        }
    }
}
