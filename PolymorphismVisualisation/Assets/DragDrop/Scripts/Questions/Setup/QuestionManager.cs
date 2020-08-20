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
    public GameObject statusMessageBox;
    public GameObject checkButton;
    public GameObject yesButton;
    public GameObject noButton;
    public DropRegion dropRegion;
    public AudioSource correctSound;
    public AudioSource incorrectSound;

    private Question currentQuestion;
    private QuestionFactory previousQuestionFactory;
    private System.Random randomGen = new System.Random();

    private Dictionary<int, List<Entity>> entities;

    private int activitySection;
    private List<QuestionFactory> questionList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setupQuestionManager(int activitySection, Language codeLanguage)
    {
        Question.setGameObjects(codeBox, questionTextBox, statusMessageBox, checkButton, yesButton, noButton, dropRegion);
        this.activitySection = activitySection;
        QuestionFactory.setCodeLanguage(codeLanguage);
        questionList = QuestionListFactory.generateQuestionList(activitySection);
        generateQuestion();
    }

    public void generateQuestion()
    {

        int numberOfQuestionTypes = questionList.Count;
        int selectedType = randomGen.Next(numberOfQuestionTypes);
        QuestionFactory factory = questionList[selectedType];
        while(previousQuestionFactory != null && factory.GetType().Equals(previousQuestionFactory.GetType()))
        {
            selectedType = randomGen.Next(numberOfQuestionTypes);
            factory = questionList[selectedType];
        }

        previousQuestionFactory = factory;
        Question question = factory.getQuestion();
        this.currentQuestion = question;
        question.loadQuestion();
    }

    private void clearQuestionRegion()
    {
        dropRegion.clearSelected();

        statusMessageBox.SetActive(false);
        checkButton.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }

    public void checkQuestion()
    {
        if (currentQuestion.checkCorrectness())
        {
            correctAnswerProcedure();
        }
        else
        {
            wrongAnswerProcedure();
        }
    }

    public void answerBoolQuestion(bool answer)
    {
        if (currentQuestion.checkYesNoAnswer(answer))
        {
            correctAnswerProcedure();
        }
        else
        {
            wrongAnswerProcedure();
        }
    }

    private void correctAnswerProcedure()
    {

        if (!GameManager.muted)
        {
            correctSound.Play();
        }

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

    private void wrongAnswerProcedure()
    {
        if (!GameManager.muted)
        {
            incorrectSound.Play();
        }
    }

    public void updateQuestion()
    {
        currentQuestion.loadQuestion();
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
        currentQuestion.objectPlaced(objectEntity);
    }

    


}
