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
        yesButton.GetComponent<Button>().interactable = false;
        noButton.SetActive(true);
        noButton.GetComponent<Button>().interactable = false;
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
        return userAnswer == correctAnswer;
    }
}
