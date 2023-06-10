using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeScriptChars {
    public char character;
    public int index;
    public bool isTyped;
}
public class TypeScriptingStructure
{
    public int id = 0;
    public string text;
    public int currentIndex = 0;
    public List<TypeScriptChars> characters = new List<TypeScriptChars>();
}

public class TypeScriptingManager : MonoBehaviour
{
    public static List<TypeScriptingStructure> SelectedTypeScriptingSentences = new List<TypeScriptingStructure>();
    public bool minigameStarted = false;
    public bool minigameShouldStart = false;
    public bool sentenceFinished = false;
    public int currentSentenceIndex = 0;

    public readonly float secondsRemaing = 120;
    public float currentSecondsRemaing = 0;

    public Coroutine timerCoroutine = null;
    public Coroutine startCoroutine = null;

    public List<GameObject> RowItems = new List<GameObject>();

    void Update()
    {
        var typeScritingCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.TYPE_SCRIPTING_CHALLENGE);
        var alertCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.ALERT);
        if (
            alertCanvas == null &&
            typeScritingCanvas == null)
        {
            minigameShouldStart = false;
            minigameStarted = false;
            return;
        }

        if (
            StateManager.SelectedMinigame == MinigameType.TYPE_SCRIPTING_CHALLENGE && 
            typeScritingCanvas != null && 
            !minigameStarted && !minigameShouldStart
            )
        {
            minigameShouldStart = true;
        }
        
        if (typeScritingCanvas != null && StateManager.SelectedMinigame == MinigameType.TYPE_SCRIPTING_CHALLENGE && !minigameStarted && minigameShouldStart)
        {
            if (startCoroutine == null)
            {
                startCoroutine = StartCoroutine(Config.Waiter(() => {
                    if (minigameShouldStart && !minigameStarted)
                    {
                       typeScritingCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("Timer").GetComponent<TextMeshProUGUI>().text = "The minigame is about to start in 10s";
                    }
                }, () => {
                    if (minigameShouldStart && !minigameStarted)
                    {
                        StartGame();
                    }
                }, 10));
            }
        }

        if (minigameStarted && currentSecondsRemaing == 0)
        {
            // We need to close this canvas in order to show the alert canvas
            GameObject.Find("TypescriptingCanvas")?.SetActive(false);
            ExitGame();
            StateManager.chatCanvasShouldRender = false;
            StateManager.OnStopDialog();
            StateManager.LastResultFromTypingscript = false;
            StateManager.SetupDialog(new List<string>(){"Time's up! :/"}, DialogType.ALERT, false);
            return;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StateManager.SelectedMinigame = MinigameType.NONE;
            ExitGame();
            return;
        }
    }

    public void StartGame()
    {
        ResetGame();
        StartTimer();
        currentSentenceIndex = 0;
        SelectedTypeScriptingSentences.Clear();
        var words = Config.GetRandomTypescriptingPhrases(10);
        for(int i = 0 ; i < words.Count; i++)
        {
            TypeScriptingStructure sentence = new TypeScriptingStructure();
            sentence.id = i;
            sentence.text = words[i];
            sentence.currentIndex = 0;
            
            for (int j = 0 ; j < sentence.text.Length; j++)
            {
                TypeScriptChars charStruct = new TypeScriptChars();
                charStruct.character = sentence.text[j];
                charStruct.index = j;
                charStruct.isTyped = false;
                sentence.characters.Add(charStruct);
            }

            SelectedTypeScriptingSentences.Add(sentence);
        }

        minigameStarted = true;
        RefreshCanvas();
    }
    
    void IncrementSentenceIndex()
    {
        if (currentSentenceIndex < SelectedTypeScriptingSentences.Count - 1) {
            currentSentenceIndex++;
            RefreshCanvas();
        }
        else
        {
            // We need to close this canvas in order to show the alert canvas
            GameObject.Find("TypescriptingCanvas")?.SetActive(false);
            ExitGame();
            StateManager.chatCanvasShouldRender = false;
            StateManager.OnStopDialog();
            StateManager.LastResultFromTypingscript = true;
            StateManager.SetupDialog(new List<string>(){"Quest completed!"}, DialogType.ALERT, false);
        }
        sentenceFinished = false;
    }

    void RefreshCanvas()
    {
        var typeScritingCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.TYPE_SCRIPTING_CHALLENGE);
        if (typeScritingCanvas == null)
        {
            return;
        }

        // Declare a new variable for a custom text. We need to check if each caracter is typed or not. Is yes, we add a green color, if not, we add a grey color
        string newTextFormatted = "";
        var currentSentence = SelectedTypeScriptingSentences[currentSentenceIndex];
        for (int i = 0; i < currentSentence.characters.Count; i++)
        {
            var currentChar = currentSentence.characters[i];
            if (currentChar.isTyped)
            {
                newTextFormatted += "<color=#00ff00ff>" + currentChar.character + "</color>";
            }
            else
            {
                newTextFormatted += "<color=#808080ff>" + currentChar.character + "</color>";
            }
        }

        // Set bold
        newTextFormatted = "<b>" + newTextFormatted + "</b>";
        
        // Update Current word
        typeScritingCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("CurrentWord").GetComponent<TextMeshProUGUI>().text = newTextFormatted;

        // Remove all Row-Item objects. Tag = Row-Item
        GameObject[] others = GameObject.FindGameObjectsWithTag("Row-Item");
        foreach (GameObject other in others)
        {
            Destroy(other);
        }

        // Get the base row on TypescriptingCanvas/PanelInfo/Row and instantiate it. Each row is a new sentence
        var baseRow = typeScritingCanvas.Canvas.gameObject.transform.Find("PanelInfo").Find("Panel").Find("Row");

        // For each sentence, we instantiate a new row and. If row is > 1, start updating the vertical position to avoid overlapping
        for (int i = 0; i < SelectedTypeScriptingSentences.Count; i++)
        {
            var currentSentenceRow = SelectedTypeScriptingSentences[i];
            var currentRow = Instantiate(baseRow, baseRow.parent);
            currentRow.name = "Row-Item";
            currentRow.tag = "Row-Item";
            currentRow.gameObject.SetActive(true);
            currentRow.localPosition = new Vector3(currentRow.localPosition.x, currentRow.localPosition.y - (i * 30), currentRow.localPosition.z);

            currentRow.Find("sentence").GetComponent<TextMeshProUGUI>().text = currentSentenceRow.text;

            // Set InProgress if sentence is the active one, pending if the index of sentence if greater then the current one, and completed if the sentence is completed
            currentRow.Find("status").GetComponent<TextMeshProUGUI>().text = currentSentenceIndex == i ? "In Progress" : currentSentenceIndex > i ? "Completed" : "Pending";

            // Set green color 50% opacity if sentence is compleed, 100% yellow is sentence is in progress, 50% opacity white if sentence is pending
            currentRow.Find("sentence").GetComponent<TextMeshProUGUI>().color = currentSentenceIndex == i ? new Color(1, 1, 1, 0.9f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            currentRow.Find("status").GetComponent<TextMeshProUGUI>().color = currentSentenceIndex == i ? new Color(1, 1, 0, 1) : currentSentenceIndex > i ? new Color(0, 1, 0, 0.5f) : new Color(1, 1, 1, 0.5f);

            RowItems.Add(currentRow.gameObject);
        }
    }

    void ResetGame()
    {
        var typeScritingCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.TYPE_SCRIPTING_CHALLENGE);

        if (typeScritingCanvas != null)
        {
           typeScritingCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("Timer").GetComponent<TextMeshProUGUI>().text = "The minigame is about to start in 10s";
           typeScritingCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("CurrentWord").GetComponent<TextMeshProUGUI>().text = "";
        }
        // Remove all Row-Item objects. Tag = Row-Item
        foreach (GameObject other in RowItems)
        {
            Destroy(other);
        }

        RowItems.Clear();
        minigameShouldStart = false;
        minigameStarted = false;
        currentSentenceIndex = 0;
        SelectedTypeScriptingSentences.Clear();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (startCoroutine != null)
        {
            StopCoroutine(startCoroutine);
            startCoroutine = null;
        }
    }

    void StartTimer()
    {
        var typeScritingCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.TYPE_SCRIPTING_CHALLENGE);
        if (typeScritingCanvas == null || timerCoroutine != null)
        {
            return;
        }

        currentSecondsRemaing = secondsRemaing;
        // Start a timer for game duration. Take account the secondsRemaing and CurrentSecondsRemaing variable. timerCoroutine available
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void ExitGame()
    {
        ResetGame();
        StateManager.chatCanvasShouldRender = false;
    }

    private IEnumerator TimerCoroutine()
    {
        var typeScritingCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.TYPE_SCRIPTING_CHALLENGE);
        if (typeScritingCanvas == null)
        {
            yield break;
        }
        // Each second, we decrease the CurrentSecondsRemaing variable and update the timer text
        while (currentSecondsRemaing > 0)
        {
            currentSecondsRemaing--;
            typeScritingCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("Timer").GetComponent<TextMeshProUGUI>().text = $"{currentSecondsRemaing.ToString()}s";
            yield return new WaitForSeconds(1);
        }
    }

    void OnGUI()
    {
        var alertCanvas = StateManager.SelectedDialogCanvas.Find(canvas => canvas.DialogType == DialogType.ALERT);
        if (StateManager.SelectedMinigame != MinigameType.TYPE_SCRIPTING_CHALLENGE && alertCanvas == null || SelectedTypeScriptingSentences.Count == 0)
        {
            return;
        }

         if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey && e.keyCode != KeyCode.None && !sentenceFinished)
            {
                var currentSentence = SelectedTypeScriptingSentences[currentSentenceIndex];
                var currentTypedChar = currentSentence.characters[currentSentence.currentIndex];
                
                if (
                    e.keyCode.ToString() == currentTypedChar.character.ToString().ToUpper() || 
                    (e.keyCode == KeyCode.Space && currentTypedChar.character == ' ')
                )
                {
                    currentTypedChar.isTyped = true;
                    RefreshCanvas();
                    currentSentence.currentIndex++;
                } else {
                    Debug.Log("Wrong character!");
                    return;
                }

                if(currentSentence.currentIndex == currentSentence.text.Length)
                {
                    Debug.Log("Sentence typed!");
                    sentenceFinished = true;
                    // Wait two seconds before incrementing the sentence index. Use Config.Waiter
                    StartCoroutine(Config.Waiter(() => {}, () => {
                        IncrementSentenceIndex();
                    }, 0.03f));
                }
            }
        }
    }   
}
