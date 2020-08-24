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

        if (variableType.Equals(dropRegion.screenEntity) && dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity))
        {
            status = "Status: Valid object placed in screen";
            result = true;
            
        } else if (!dropRegion.screenEntity.Equals(variableType))
        {
            status = "Selected screen does not match variable type of code snippet";
        } else if(dropRegion.screenEntity != dropRegion.objectEntity)
        {
            status = "Selected object is not compatible with variable type";
        }
        updateStatus(status, result);

        return result;
    }

}
