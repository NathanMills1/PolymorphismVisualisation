using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicInheritanceQuestion : Question
{

    public BasicInheritanceQuestion(string codeText, string questionText, int numberOfCodeLines, Entity variableType)
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

    protected override bool performQuestionSpecificCheck()
    {
        string status;
        Color colour;
        try
        {
            if (dropRegion.objectEntity == null)
            {
                status = "Compiler Error: Null reference to object";
                colour = Color.red;
                statusMessageBox.GetComponent<StatusHandler>().updateStatus(status, colour);
            }
             else if (variableType.Equals(dropRegion.screenEntity) && dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity) && dropRegion.objectEntity != variableType)
            {
                status = "Status: Valid object placed in screen";
                colour = new Color32(33, 171, 74, 255);
                statusMessageBox.GetComponent<StatusHandler>().updateStatus(status, colour);
                return true;
            }

        } catch (System.NullReferenceException)
        {
            //Screen or object not placed, can't be correct
        }
        return false;
    }

}
