using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadmap : MonoBehaviour
{
    public string mapname;
    public void loadmap1()
    {
        // Load scene for start
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(mapname,  UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
     public void button_exit()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void button_back()
    {
       var canvas = StateManager.SelectedDialogCanvas.Find(x => x.DialogType == DialogType.MENU);
        if (canvas != null)
        {
            StateManager.chatCanvasShouldRender = false;
            StateManager.OnStopDialog();
        }
    }
}
