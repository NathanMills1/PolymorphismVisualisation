using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicInheritanceQuestion : Question
{

    public BasicInheritanceQuestion(string codeText, string questionText, int numberOfCodeLines, Entity variableType)
    {
        this.questionID = 2;
        this.codeText = codeText;
        this.questionText = questionText;
        this.numberOfCodeLines = numberOfCodeLines;
        this.variableType = variableType;
        variableCodePosition = 0;
        objectCodePosition = -1;

        this.usesCheckButton = true;
    }

    public override void loadQuestion()
    {
        codeBox.GetComponentInChildren<TextMeshProUGUI>().text = createCodeText();
        questionTextBox.GetComponentInChildren<TextMeshProUGUI>().text = createQuestionText();

        statusText.GetComponent<TextMeshProUGUI>().text = "";
        statusText.GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
    }

    protected override string createQuestionText()
    {
        return questionText.Replace("variableType", variableType.identity.name);
    }

    protected override bool performQuestionSpecificCheck()
    {
        string status = "";
        bool result = false;
        if (variableType.Equals(dropRegion.screenEntity) && dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity) && dropRegion.objectEntity != variableType)
        {
            status = "Status: Valid instance assigned to variable type";
            
            result = true;
        }
        else if (!dropRegion.screenEntity.Equals(variableType))
        {
            status = "Selected screen does not match variable type of code snippet";
        }
        else if (dropRegion.screenEntity != dropRegion.objectEntity)
        {
            status = "Selected instance is not compatible with variable type";
        } else if(dropRegion.screenEntity == dropRegion.objectEntity)
        {
            status = "While the placement is valid, it is not what the question is asking";
        }

        updateStatus(status, result);
        return result;
    }

    public override string getExpectedScreenAndObject()
    {
        return variableType.identity.name + ",child";
    }

}
