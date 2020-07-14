using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateQuestion()
    {
        int numberOfQuestionTypes = 1;
        int selectedType = randomGen.Next(numberOfQuestionTypes);


        QuestionFactory factory = null;
        switch (selectedType)
        {
            case 0:
                factory = new BasicQuestionFactory(InheritanceGenerator.selectedEntitiesByGeneration);
                break;
        }

        Question question = factory.getQuestion();
        this.currentQuestion = question;
        question.loadQuestion();
    }

    private void clearQuestionRegion()
    {
        codeBox.SetActive(false);
        questionTextBox.SetActive(false);
        statusMessageBox.SetActive(false);
        statusMessageBox.GetComponentInChildren<TextMeshProUGUI>().text = "Status: ";
        checkButton.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        dropRegion.clearSelected();
    }

    public void checkQuestion()
    {
        if (currentQuestion.checkCorrectness())
        {
            System.Threading.Thread.Sleep(3000);
            clearQuestionRegion();
            generateQuestion();
        }
    }

    public void updateQuestion()
    {
        currentQuestion.loadQuestion();
    }


}
