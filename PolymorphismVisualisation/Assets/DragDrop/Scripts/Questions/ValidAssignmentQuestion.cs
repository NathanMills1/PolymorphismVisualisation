using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidAssignmentQuestion : Question
{
    private bool correctAnswer;

    public ValidAssignmentQuestion(string codeText, string questionText, int numberOfCodeLines, Entity variableType, Entity objectType, bool correctAnswer, int variablePosition, int objectPosition)
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
        return newCodeText.Replace("selectedType", objectType.identity.name);
    }

    public override bool checkYesNoAnswer(bool userAnswer)
    {
        return userAnswer == correctAnswer;
    }
}
