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
}

public enum MinigameType {
    NONE,
    FETCH,
    CODE_CHALLENGE,
    PUZZLE
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

[System.Serializable]
public class QuestionPuzzle
{
    public string code;
    public Dictionary<string, string> order;
}

public class QuestionPuzzleData {
    public List<QuestionPuzzle> Questions;
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

    public static Question GetRandomQuestion(QuestionDifficulty difficulty = QuestionDifficulty.NONE) {
        List<Question> questions = GetQuestions();
        List<Question> filteredQuestions = new List<Question>();
        Question randomQuestion = new Question();

        filteredQuestions = 
            difficulty != QuestionDifficulty.NONE ? questions.Where(q => q.difficulty == difficulty).ToList() : questions;

        randomQuestion = filteredQuestions[Random.Range(0, filteredQuestions.Count)];
        return randomQuestion;
    }

    public static QuestionPuzzle GetRandomPuzzleQuestion() {
        List<QuestionPuzzle> questions = GetPuzzleQuestions();
        return questions[Random.Range(0, questions.Count)];
    }

    public static System.Action onDelegate;
    public static IEnumerator Waiter(System.Action callback, System.Action onComplete = null, int waitTime = 1)
    {
        callback?.Invoke();
        yield return new WaitForSeconds(waitTime);
        onComplete?.Invoke();
    }
}