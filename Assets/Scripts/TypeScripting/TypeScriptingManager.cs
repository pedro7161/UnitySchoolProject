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

    void Update()
    {
        if (
            StateManager.SelectedMinigame != MinigameType.TYPE_SCRIPTING_CHALLENGE && 
            StateManager.SelectedDialogCanvas?.DialogType != DialogType.ALERT && 
            StateManager.SelectedDialogCanvas?.DialogType != DialogType.TYPE_SCRIPTING_CHALLENGE)
        {
            minigameShouldStart = false;
            minigameStarted = false;
            return;
        }

        if (
            StateManager.SelectedMinigame == MinigameType.TYPE_SCRIPTING_CHALLENGE && 
            StateManager.SelectedDialogCanvas?.DialogType == DialogType.TYPE_SCRIPTING_CHALLENGE && !minigameStarted && !minigameShouldStart)
        {
            minigameShouldStart = true;
        }
        
        if (StateManager.SelectedDialogCanvas != null && StateManager.SelectedMinigame == MinigameType.TYPE_SCRIPTING_CHALLENGE && !minigameStarted && minigameShouldStart)
        {
            StartCoroutine(Config.Waiter(() => {
                if (minigameShouldStart && !minigameStarted)
                {
                    StateManager.SelectedDialogCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("Timer").GetComponent<TextMeshProUGUI>().text = "The minigame is about to start in 10s";
                }
            }, () => {
                if (minigameShouldStart && !minigameStarted)
                {
                    Debug.Log("Starting a new minigame type scripting challenge");
                    StartGame();
                    minigameShouldStart = false;
                }
            }, 10));
        }

        if (minigameStarted && currentSecondsRemaing == 0)
        {
            StateManager.SelectedDialogCanvas.Canvas.enabled = false;
            ExitGame();
            Debug.Log("Time is up!");
            StateManager.chatCanvasShouldRender = false;
            StateManager.OnStopDialog();
            StateManager.LastResultFromTypingscript = false;
            StateManager.SetupDialog(new List<string>(){"Time is up :/"}, DialogType.ALERT, false);
        }
    }

    public void StartGame()
    {
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
            
            StateManager.SelectedDialogCanvas.Canvas.enabled = false;
            ExitGame();
            Debug.Log("Time is up!");
            StateManager.chatCanvasShouldRender = false;
            StateManager.OnStopDialog();
            StateManager.LastResultFromTypingscript = true;
            StateManager.SetupDialog(new List<string>(){"Quest completed!"}, DialogType.ALERT, false);
        }
        sentenceFinished = false;
    }

    void RefreshCanvas()
    {
        if (StateManager.SelectedDialogCanvas == null)
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
        StateManager.SelectedDialogCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("CurrentWord").GetComponent<TextMeshProUGUI>().text = newTextFormatted;

        // Remove all Row-Item objects. Tag = Row-Item
        GameObject[] others = GameObject.FindGameObjectsWithTag("Row-Item");
        foreach (GameObject other in others)
        {
            Destroy(other);
        }

        // Get the base row on TypescriptingCanvas/PanelInfo/Row and instantiate it. Each row is a new sentence
        var baseRow = StateManager.SelectedDialogCanvas.Canvas.gameObject.transform.Find("PanelInfo").Find("Panel").Find("Row");

        // For each sentence, we instantiate a new row and. If row is > 1, start updating the vertical position to avoid overlapping
        for (int i = 0; i < SelectedTypeScriptingSentences.Count; i++)
        {
            var currentSentenceRow = SelectedTypeScriptingSentences[i];
            var currentRow = Instantiate(baseRow, baseRow.parent);
            currentRow.name = "Row-item";
            currentRow.tag = "Row-Item";
            currentRow.gameObject.SetActive(true);
            currentRow.localPosition = new Vector3(currentRow.localPosition.x, currentRow.localPosition.y - (i * 30), currentRow.localPosition.z);

            currentRow.Find("sentence").GetComponent<TextMeshProUGUI>().text = currentSentenceRow.text;

            // Set InProgress if sentence is the active one, pending if the index of sentence if greater then the current one, and completed if the sentence is completed
            currentRow.Find("status").GetComponent<TextMeshProUGUI>().text = currentSentenceIndex == i ? "In Progress" : currentSentenceIndex > i ? "Completed" : "Pending";

            // Set green color 50% opacity if sentence is compleed, 100% yellow is sentence is in progress, 50% opacity white if sentence is pending
            currentRow.Find("sentence").GetComponent<TextMeshProUGUI>().color = currentSentenceIndex == i ? new Color(1, 1, 1, 0.9f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            currentRow.Find("status").GetComponent<TextMeshProUGUI>().color = currentSentenceIndex == i ? new Color(1, 1, 0, 1) : currentSentenceIndex > i ? new Color(0, 1, 0, 0.5f) : new Color(1, 1, 1, 0.5f);
        }
    }

    void ResetGame()
    {
        // Remove all Row-Item objects. Tag = Row-Item
        GameObject[] others = GameObject.FindGameObjectsWithTag("Row-Item");
        foreach (GameObject other in others)
        {
            Destroy(other);
        }
        minigameShouldStart = false;
        minigameStarted = false;
        currentSentenceIndex = 0;
        SelectedTypeScriptingSentences.Clear();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    void StartTimer()
    {
        if (StateManager.SelectedDialogCanvas == null || timerCoroutine != null)
        {
            return;
        }

        currentSecondsRemaing = secondsRemaing;
        // Start a timer for game duration. Take account the secondsRemaing and CurrentSecondsRemaing variable. timerCoroutine available
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void ExitGame()
    {
        StateManager.chatCanvasShouldRender = false;
        //StateManager.SelectedMinigame = MinigameType.NONE;
        ResetGame();
    }

    private IEnumerator TimerCoroutine()
    {
        // Each second, we decrease the CurrentSecondsRemaing variable and update the timer text
        while (currentSecondsRemaing > 0)
        {
            currentSecondsRemaing--;
            StateManager.SelectedDialogCanvas.Canvas.gameObject.transform.Find("PanelMainText").Find("Timer").GetComponent<TextMeshProUGUI>().text = $"{currentSecondsRemaing.ToString()}s";
            yield return new WaitForSeconds(1);
        }
    }

    void OnGUI()
    {
        if (StateManager.SelectedMinigame != MinigameType.TYPE_SCRIPTING_CHALLENGE && StateManager.SelectedDialogCanvas?.DialogType != DialogType.ALERT || SelectedTypeScriptingSentences.Count == 0)
        {
            return;
        }

         if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey && e.keyCode != KeyCode.None && !sentenceFinished)
            {
                if (e.keyCode == KeyCode.Escape)
                {
                    ExitGame();
                    return;
                }
                
                var currentSentence = SelectedTypeScriptingSentences[currentSentenceIndex];
                var currentTypedChar = currentSentence.characters[currentSentence.currentIndex];

                Debug.Log("Current char to be typed: " + currentTypedChar.character.ToString().ToUpper() + " - " + e.keyCode);
                
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
