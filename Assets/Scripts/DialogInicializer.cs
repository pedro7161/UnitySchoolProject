using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[Serializable]
public class DialogCanvasStructure {
    public DialogType DialogType = DialogType.DIALOG;
    public Canvas Canvas = null;
    public TextMeshProUGUI OutputSentences = null;
    public GameObject[] Buttons = null;
    public TextMeshProUGUI CanvasTitle = null;
}

public class DialogInicializer : MonoBehaviour
{
    public List<DialogCanvasStructure> dataset = new List<DialogCanvasStructure>();
    
    void Start() {
        var dialogsDistinct = Enum.GetValues(typeof(DialogType));
        foreach (DialogType dialog in dialogsDistinct) {
            if (dataset.Count(canvas => canvas.DialogType == dialog) > 1) {
                 throw new Exception($"Duplicate Canvas with Dialog type '{dialog}' found");
            }
        }

        StateManager.SetDialogCanvasData(new HashSet<DialogCanvasStructure>(dataset));
    }
}
