using System;
using System.Linq;
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
    public int currentSentenceIndex = 0;
    
    void Start()
    {
        GenerateSentences();
     }

    void Update()
    {
        if (StateManager.SelectedMinigame != MinigameType.TYPE_SCRIPTING_CHALLENGE && StateManager.SelectedDialogCanvas?.DialogType != DialogType.ALERT)
        {
            return;
        }

        handleUserInput();
    }
    public void handleUserInput()
    {
        // Get current character pressed by user and then compare it with the current character in the current sentence.
        // If the character is the same, then mark it as typed and move to the next character.
        if (Input.anyKeyDown)
        {
            var currentSentence = SelectedTypeScriptingSentences[currentSentenceIndex];
            var currentTypedChar = currentSentence.characters[currentSentence.currentIndex];
            
            if (Input.GetKeyDown(currentTypedChar.character.ToString()))
            {
                currentTypedChar.isTyped = true;
                currentSentence.currentIndex++;
            } else {
                Debug.Log("Wrong character!");
                return;
            }

            if (currentSentence.currentIndex + 1 == currentSentence.text.Length)
            {
                currentSentenceIndex++;
            }

            if (currentSentenceIndex == SelectedTypeScriptingSentences.Count - 1)
            {
                Debug.Log("All sentences are typed!");
            }
        }
    }

    public void GenerateSentences()
    {
        var words = Config.GetRandomTypescriptingPhrases(20);
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
    }
}
