using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class loadmap : MonoBehaviour
{

    void Update()
    {
        // if scene name is menu, then set cursor state to true
        if (SceneManager.GetActiveScene().name == "menu")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
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
    public void LoadMap(string mapname)
    {
        Resources.UnloadUnusedAssets();
        StateManager.OnStopDialog();
        StateManager.chatCanvasShouldRender = false;
        LevelManager.shouldShowWelcomeMessage = mapname != "Mechanics Test";
        // set cursor state to true
        if (mapname == "menu")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mapname,  UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            StartCoroutine(DisableButtonsOnLoadCoroutine(mapname));
        }
    }

    IEnumerator DisableButtonsOnLoadCoroutine(string mapname)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            button.gameObject.GetComponent<Button>().enabled = false;
            // set a disabled style for the button and text
            button.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        var loadingText = GameObject.Find("LoadingText")?.GetComponent<TextMeshProUGUI>();
        // SET STYLE TO H1 using enumerator default
            loadingText.text = "print(\"Loading...\"))";
        // wait 1 second
        yield return new WaitForSeconds(1);
        // load scene
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(mapname,  UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
