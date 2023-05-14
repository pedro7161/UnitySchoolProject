using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum DialogType 
{
    DIALOG,
    CODE_CHALLENGE,
    ALERT
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


public static class Config
{

    public static List<string> templateSentences = new List<string> 
    {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce in mauris nec tortor iaculis luctus sed vitae turpis. Etiam ut dignissim enim. Pellentesque auctor leo id viverra dictum.",
        "Testing sentence",
        "Testing sentence",
        "Testing sentence",
        "Testing sentence"
    };

    private static List<Question> questions = new List<Question>();

    // should initialize and preload questions
    public static List<Question> GetQuestions() {
        if (questions.Count == 0) {
            TextAsset jsonFile = Resources.Load<TextAsset>("questions");
            var myObject = JsonUtility.FromJson<QuestionsData>(jsonFile.text);

            questions.AddRange(myObject.Questions);
        }
        return questions;
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

    public static System.Action onDelegate;
    public static IEnumerator Waiter(System.Action callback, System.Action onComplete = null, int waitTime = 1)
    {
        callback?.Invoke();
        yield return new WaitForSeconds(waitTime);
        onComplete?.Invoke();
    }
}