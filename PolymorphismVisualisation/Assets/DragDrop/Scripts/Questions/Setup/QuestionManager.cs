using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public GameObject codeBox;
    public GameObject questionTextBox;
    public GameObject statusText;
    public GameObject checkButton;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject continueButton;
    public DropRegion dropRegion;
    public AudioSource correctSound;
    public AudioSource incorrectSound;


    private Question currentQuestion;
    private QuestionFactory previousQuestionFactory;

    private Dictionary<int, List<Entity>> entities;

    private List<QuestionFactory> questionList;

    private static float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void setupQuestionManager(int activitySection, Language codeLanguage)
    {
        Question.setGameObjects(codeBox, questionTextBox, statusText, checkButton, yesButton, noButton, continueButton, dropRegion, correctSound, incorrectSound);
        QuestionFactory.setCodeLanguage(codeLanguage);
        questionList = QuestionListFactory.generateQuestionList(activitySection);
    }

    public void generateQuestion()
    {

        int numberOfQuestionTypes = questionList.Count;
        int selectedType = RandomGen.next(numberOfQuestionTypes);
        QuestionFactory factory = questionList[selectedType];
        while(previousQuestionFactory != null && factory.GetType().Equals(previousQuestionFactory.GetType()))
        {
            selectedType = RandomGen.next(numberOfQuestionTypes);
            factory = questionList[selectedType];
        }

        previousQuestionFactory = factory;
        Question question = factory.getQuestion();
        this.currentQuestion = question;
        question.loadQuestion();

        time = 0f;
    }

    private void clearQuestionRegion()
    {
        dropRegion.clearSelected();

        checkButton.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        continueButton.SetActive(false);
    }

    public void checkQuestion()
    {
        currentQuestion.checkCorrectness();
    }

    public void answerBoolQuestion(bool answer)
    {
        bool correctAnswer = currentQuestion.checkYesNoAnswer(answer) ? answer : !answer;
    }

    public void correctAnswerProcedure()
    {

        clearQuestionRegion();
        if (GameManager.updateSectionProgress())
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            generateQuestion();
        }
        
    }

    public void updateQuestion()
    {
        if(currentQuestion != null)
        {
            currentQuestion.loadQuestion();
        }
    }

    public void screenPlaced(Entity screen)
    {
        if(currentQuestion != null) 
        {
            currentQuestion.screenPlaced(screen);
        }
        
    }

    public void objectPlaced(Entity objectEntity)
    {
        if (currentQuestion != null)
        {
            currentQuestion.objectPlaced(objectEntity);
        }
    }

    public static void resetTimer()
    {
        time = 0;
    }

    


}
