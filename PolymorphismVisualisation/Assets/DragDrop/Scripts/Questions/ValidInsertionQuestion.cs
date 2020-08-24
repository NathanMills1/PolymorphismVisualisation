using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidInsertionQuestion : Question
{
    private bool correctAnswer;

    public ValidInsertionQuestion(string codeText, string questionText, int numberOfCodeLines, Entity variableType, Entity objectType, bool correctAnswer, int variablePosition, int objectPosition)
    {
        this.codeText = codeText;
        this.questionText = questionText;
        this.numberOfCodeLines = numberOfCodeLines;
        this.variableType = variableType;
        this.objectType = objectType;
        this.correctAnswer = correctAnswer;

        variableCodePosition = variablePosition;
        objectCodePosition = objectPosition;

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
        string insertedName = camelCase(objectType);
        return newCodeText.Replace("InsertedType",objectType.identity.name).Replace("InsertedName", insertedName);
    }

    public override bool checkYesNoAnswer(bool userAnswer)
    {
        string status = "";
        bool result = false;

        if (correctAnswer != userAnswer)
        {
            status = "Incorrect. \nTry and see if the object inherits from the variable type";
        }
        else
        {
            if (correctAnswer)
            {
                status = "Correct. " + objectType.identity.name + " can be assigned to variable type of " + variableType.identity.name;
            }
            else
            {
                status = "Correct. " + objectType.identity.name + " cannot be assigned to variable type of " + variableType.identity.name;
            }

            result = true;
        }
        updateStatus(status, result);

        return result;
    }
}
