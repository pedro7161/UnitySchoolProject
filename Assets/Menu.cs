using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Canvas menu;

    private void Start()
    {
    }

    private void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.Escape) && StateManager.SelectedDialogCanvas.TrueForAll(x => x.DialogType == DialogType.MENU || x.DialogType == DialogType.QUEST) && !LevelManager.shouldShowWelcomeMessage
        )
        {
            var canvas = StateManager.SelectedDialogCanvas.Find(x => x.DialogType == DialogType.MENU);
            if (canvas != null && canvas.Canvas.gameObject.activeSelf)
            {
                StateManager.chatCanvasShouldRender = false;
                StateManager.OnStopDialog();
            }
            else
            {
                Debug.Log("Menu - setup menu dialog");
                StateManager.SetupDialog(new List<string>(){""}, DialogType.MENU, false);
            }
        }
    }
}