using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private System.Random randomGen = new System.Random();

    private Dictionary<int, List<Entity>> entities;

    // Start is called before the first frame update
    void Start()
    {
        Question.setGameObjects(codeBox, questionTextBox, statusMessageBox, checkButton, yesButton, noButton, dropRegion);
        QuestionFactory.setCodeLanguage(Language.CSharp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateQuestion()
    {

        int numberOfQuestionTypes = 4;
        int selectedType = randomGen.Next(2, numberOfQuestionTypes);

        QuestionFactory factory = null;
        switch (selectedType)
        {
            case 0:
                factory = new BasicQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
            case 1:
                factory = new ValidMethodCallQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
            case 2:
                factory = new ValidInsertionQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
            case 3:
                factory = new CollectionCreationQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
        }

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
        correctSound.Play();
        clearQuestionRegion();
        generateQuestion();
    }

    private void wrongAnswerProcedure()
    {
        incorrectSound.Play();
    }

    public void updateQuestion()
    {
        currentQuestion.loadQuestion();
    }

    public void screenPlaced(Entity screen)
    {
        currentQuestion.screenPlaced(screen);
    }

    public void objectPlaced(Entity objectEntity)
    {
        currentQuestion.objectPlaced(objectEntity);
    }

    


}
