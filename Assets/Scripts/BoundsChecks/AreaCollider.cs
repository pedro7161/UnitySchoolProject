using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas warningCanvas;
    public LevelEnum currentZone = LevelEnum.LEVEL_1;
    public LevelEnum previousZone = LevelEnum.LEVEL_1;
    public static string zoneName = string.Empty;
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
                LevelManager.setAudioLevel(LevelEnum.LEVEL_1);
                change_skybox.Change_Skybox(2);
                break;
            case "Zone2":
                currentZone = LevelEnum.LEVEL_2;
                
                LevelManager.setAudioLevel(LevelEnum.LEVEL_2);
                change_skybox.Change_Skybox(0);
                break;
            case "Zone3":
                currentZone = LevelEnum.LEVEL_3;
             
                LevelManager.setAudioLevel(LevelEnum.LEVEL_3);
                change_skybox.Change_Skybox(1);
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
    }
}
