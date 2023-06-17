using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum DialogType 
{
    DIALOG,
    CODE_CHALLENGE,
    ALERT,
    PUZZLE,
    QUEST,
    TYPE_SCRIPTING_CHALLENGE,
    MENU,
}

public enum MinigameType {
    NONE,
    CODE_CHALLENGE,
    PUZZLE,
    TYPE_SCRIPTING_CHALLENGE,
}

public enum QuestionDifficulty {
    EASY,
    MEDIUM,
    HARD,
    NONE = -1
}

[System.Serializable]
public class Question
{
    public string title;
    public string code;
    public List<string> answers;
    public int correct_answer;
    public QuestionDifficulty difficulty;
}

[System.Serializable]
public class QuestionsData
{
    public List<Question> Questions;
}

// Question Puzzle Structure
[System.Serializable]
public class PuzzleQuestionsOrder
{
    public string sphere;
    public string capsule;
    public string cube;
    public string cylinder;
}
[System.Serializable]
public class QuestionPuzzle
{
    public string code;
    public PuzzleQuestionsOrder PuzzleQuestionsOrder;
    public QuestionDifficulty Difficulty;
}

public class QuestionPuzzleData {
    public List<QuestionPuzzle> Questions;
}

[System.Serializable]
public class TypescriptingData {
    public List<string> Words;
}

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


public static class Config
{

    public static List<string> templateSentences = new List<string> 
    {
        "Welcome to the game!",
        "To your Right you can see an example of a code challenge,just talk to the red robot and he will give you a challenge",
        "To your Left you can see two Houses, just talk to the Yellow robot and he will give you a fetch challenge",
        "Inside the other building you can see a Pink robot, just talk to him and he will give you a triple xxx challenge",
        "Have Fun!!!!"
    };

    private static List<Question> questions = new List<Question>();

    // should initialize and preload questions
    public static List<Question> GetQuestions() {
        if (questions.Count == 0) {
            TextAsset jsonFile = Resources.Load<TextAsset>("code-challenge-questions");
            var myObject = JsonUtility.FromJson<QuestionsData>(jsonFile.text);

            questions.AddRange(myObject.Questions);
        }
        return questions;
    }
    
    public static List<QuestionPuzzle> GetPuzzleQuestions() {
        TextAsset jsonFile = Resources.Load<TextAsset>("puzzle-challenge-questions");
        var myObject = JsonUtility.FromJson<QuestionPuzzleData>(jsonFile.text);

        return myObject.Questions;
    }

    public static List<string> GetTypescriptingPhrases() {
        TextAsset jsonFile = Resources.Load<TextAsset>("typescripting-challenge-phrases");
        var myObject = JsonUtility.FromJson<TypescriptingData>(jsonFile.text);

        return myObject.Words;
    }

    public static Question GetRandomQuestion(QuestionDifficulty difficulty = QuestionDifficulty.NONE) {
        List<Question> questions = GetQuestions();
        List<Question> filteredQuestions = new List<Question>();
        Question randomQuestion = new Question();

        filteredQuestions = 
            difficulty != QuestionDifficulty.NONE ? questions.Where(q => q.difficulty == difficulty).ToList() : questions;

        randomQuestion = filteredQuestions[Random.Range(0, filteredQuestions.Count)];
        Debug.Log("Random Question: " + randomQuestion.answers[randomQuestion.correct_answer] + " - " + randomQuestion.answers);
        return randomQuestion;
    }

    public static QuestionPuzzle GetRandomQuestionPuzzle() {
        List<QuestionPuzzle> questions = GetPuzzleQuestions();
        QuestionPuzzle randomQuestion = new QuestionPuzzle();

        return questions[Random.Range(0, questions.Count)];
    }

    public static List<string> GetRandomTypescriptingPhrases(int amount) {
        List<string> phrases = GetTypescriptingPhrases();
        List<string> randomPhrases = new List<string>();

        for (int i = 0; i < amount; i++) {
            randomPhrases.Add(phrases[Random.Range(0, phrases.Count)]);
        }

        return randomPhrases;
    }

    public static System.Action onDelegate;
    public static IEnumerator Waiter(System.Action callback, System.Action onComplete = null, float waitTime = 1)
    {
        callback?.Invoke();
        yield return new WaitForSeconds(waitTime);
        onComplete?.Invoke();
    }
}
