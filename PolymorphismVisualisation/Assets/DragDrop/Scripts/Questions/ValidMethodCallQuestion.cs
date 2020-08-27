using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidMethodCallQuestion : Question
{
    private bool correctAnswer;
    private string selectedMethod;
    private Entity childType;

    public ValidMethodCallQuestion(string codeText, string questionText, int numberOfCodeLines, Entity variableType, Entity childType, string selectedMethod, bool correctAnswer)
    {
        this.questionID = 6;
        this.codeText = codeText;
        this.questionText = questionText;
        this.numberOfCodeLines = numberOfCodeLines;
        this.variableType = variableType;
        this.childType = childType;
        this.selectedMethod = selectedMethod;
        this.correctAnswer = correctAnswer;

        variableCodePosition = 0;
        objectCodePosition = -1;

    }

    public override void loadQuestion()
    {
        codeBox.GetComponentInChildren<TextMeshProUGUI>().text = createCodeText();
        questionTextBox.GetComponentInChildren<TextMeshProUGUI>().text = createQuestionText();

        yesButton.SetActive(true);
        noButton.SetActive(true);

        statusText.GetComponent<TextMeshProUGUI>().text = "";
        statusText.GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
    }

    protected override string createQuestionText()
    {
        return questionText;
    }

    protected override string performQuestionSpecificCodeSwaps(string newCodeText)
    {
        return newCodeText.Replace("SelectedMethod", selectedMethod).Replace("ChildType", childType.identity.name);
    }

    public override bool checkYesNoAnswer(bool userAnswer)
    {
        string status = "";
        bool result = false;

        if (correctAnswer != userAnswer)
        {
            status = "Incorrect. \nEnsure that the given method appears within the screen of the given variable type.";
        }
        else
        {
            if (correctAnswer)
            {
                status = "Correct. Given method can be called";
            }
            else
            {
                status = "Correct. Given method cannot be called, due to being a method of the child class";
            }
            result = true;
        }
        updateStatus(status, result);

        return result;
    }
}
