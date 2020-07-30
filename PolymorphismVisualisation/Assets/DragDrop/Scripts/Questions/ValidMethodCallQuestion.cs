﻿using System.Collections;
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
        return userAnswer == correctAnswer;
    }
}
