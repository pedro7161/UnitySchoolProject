using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToScene : MonoBehaviour
{
    public string sceneName = "Mechanics Test";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (StateManager.isDialogRunning || StateManager.SelectedMinigame != MinigameType.NONE)
        {
            return;
        }
        // Load scene
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName,  UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
