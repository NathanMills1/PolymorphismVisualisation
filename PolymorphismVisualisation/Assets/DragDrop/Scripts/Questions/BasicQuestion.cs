using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicQuestion : Question
{

    public BasicQuestion(string codeText, string questionText, int numberOfCodeLines, Entity variableType)
    {
        this.codeText = codeText;
        this.questionText = questionText;
        this.numberOfCodeLines = numberOfCodeLines;
        this.variableType = variableType;
    }

    public override void loadQuestion()
    {
        codeBox.SetActive(true);
        setCodeBoxHeight();
        codeBox.GetComponentInChildren<TextMeshProUGUI>().text = createCodeText();

        questionTextBox.SetActive(true);
        questionTextBox.GetComponentInChildren<TextMeshProUGUI>().text = createQuestionText();

        checkButton.SetActive(true);
    }

    protected override string createQuestionText()
    {
        return questionText.Replace("variableType", variableType.identity.name);
    }

    protected override string performQuestionSpecificSwaps(string newCodeText)
    {
        return newCodeText.Replace("ParentType", variableType.parent.identity.name);
    }

    public override bool checkCorrectness()
    {
        TextMeshProUGUI textBox = statusMessageBox.GetComponentInChildren<TextMeshProUGUI>();
        if (variableType.Equals(dropRegion.screenEntity) && dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity))
        {
            textBox.text = "Valid object placed in screen";
            return true;
        } 
        else if (!variableType.Equals(dropRegion.screenEntity))
        {
            textBox.text = "Screen placed does not match variable type in question";
        }
        else if(dropRegion.screenEntity == null || dropRegion.objectEntity == null)
        {
            textBox.text = "Compiler Error: Null reference exception to object";
        }
        else if (!dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity))
        {
            textBox.text = "Compiler Error: " + dropRegion.objectEntity.identity.name + " does not inherit from " + dropRegion.screenEntity.identity.name;
        }

        return false;
    }
}
