using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas warningCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateLevelMusic() {
        var gameObjectName = this.gameObject.name;
        switch (gameObjectName) {
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
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            updateLevelMusic();
            //warningCanvas.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            //warningCanvas.enabled = false;
        }
    }
}
