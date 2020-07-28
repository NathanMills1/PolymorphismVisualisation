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
        variableCodePosition = 0;
        objectCodePosition = -1;
    }

    public override void loadQuestion()
    {
        codeBox.GetComponentInChildren<TextMeshProUGUI>().text = createCodeText();
        questionTextBox.GetComponentInChildren<TextMeshProUGUI>().text = createQuestionText();

        statusMessageBox.GetComponentInChildren<TextMeshProUGUI>().text = "Status: ";
        statusMessageBox.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        statusMessageBox.SetActive(true);
        checkButton.SetActive(true);
    }

    protected override string createQuestionText()
    {
        return questionText.Replace("variableType", variableType.identity.name);
    }

    public override bool performQuestionSpecificCheck(TextMeshProUGUI textBox)
    {
        try
        {
            if (variableType.Equals(dropRegion.screenEntity) && dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity))
            {
                textBox.text = "Status: Valid object placed in screen";
                textBox.color = Color.green;
                return true;
            }
            else if (!variableType.Equals(dropRegion.screenEntity))
            {
                textBox.text = "Status: Screen does not match variable type in question";
                textBox.color = Color.red;
            }
        } catch (System.NullReferenceException)
        {
            //Screen or object not placed, can't be correct
        }
        return false;
    }

}
