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

        int numberOfQuestionTypes = 2;
        int selectedType = randomGen.Next(numberOfQuestionTypes);

        QuestionFactory factory = null;
        switch (selectedType)
        {
            case 0:
                factory = new BasicQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
            case 1:
                factory = new ValidMethodCallQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
        }

        Question question = factory.getQuestion();
        this.currentQuestion = question;
        question.loadQuestion();
    }

    private void clearQuestionRegion()
    {
        statusMessageBox.SetActive(false);
        statusMessageBox.GetComponentInChildren<TextMeshProUGUI>().text = "Status: ";
        statusMessageBox.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        checkButton.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        dropRegion.clearSelected();
    }

    public void checkQuestion()
    {
        if (currentQuestion.checkCorrectness())
        {
            System.Threading.Thread.Sleep(1000);
            clearQuestionRegion();
            generateQuestion();
        }
    }

    public void updateQuestion()
    {
        currentQuestion.loadQuestion();
        currentQuestion.checkCorrectness();
    }

    public void screenPlaced(Entity screen)
    {
        currentQuestion.screenPlaced(screen);
    }


}
