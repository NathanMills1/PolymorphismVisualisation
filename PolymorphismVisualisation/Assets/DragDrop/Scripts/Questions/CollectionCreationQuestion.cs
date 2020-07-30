using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCreationQuestion : Question
{
    private bool correctAnswer;
    private Entity child1;
    private Entity child2;

    public CollectionCreationQuestion(string codeText, string questionText, int numberOfCodeLines, Entity child1, Entity child2, int variablePosition, int objectPosition)
    {
        this.codeText = codeText;
        this.questionText = questionText;
        this.numberOfCodeLines = numberOfCodeLines;
        this.objectType = objectType;
        this.child1 = child1;
        this.child2 = child2;

        variableCodePosition = variablePosition;
        objectCodePosition = objectPosition;

    }

    public override void loadQuestion()
    {
        codeBox.GetComponentInChildren<TextMeshProUGUI>().text = createCodeText();
        questionTextBox.GetComponentInChildren<TextMeshProUGUI>().text = createQuestionText();

        statusMessageBox.SetActive(true);
        checkButton.SetActive(true);
    }

    protected override string createQuestionText()
    {
        return questionText;
    }

    protected override string performQuestionSpecificCodeSwaps(string newCodeText)
    {
        string containerType = dropRegion.screenEntity != null ? dropRegion.screenEntity.identity.name : "______";
        string child1Name = camelCase(child1);
        string child2Name = camelCase(child2);
        return newCodeText.Replace("ContainerType", containerType)
            .Replace("Child1Type", child1.identity.name).Replace("Child1Name", child1Name)
            .Replace("Child2Type", child2.identity.name).Replace("Child2Name", child2Name);
    }

    protected override bool performQuestionSpecificCheck()
    {
        string status;
        Color colour;
        if(dropRegion.screenEntity != null)
        {
            if (!child1.determineIfChildOf(dropRegion.screenEntity))
            {
                status = "Status: " + child1.identity.name + " does not inherit from " + dropRegion.screenEntity.identity.name;
                colour = Color.red;
                statusMessageBox.GetComponent<StatusHandler>().updateStatus(status, colour);
                return false;
            } 
            else if (!child2.determineIfChildOf(dropRegion.screenEntity))
            {
                status = "Status: " + child2.identity.name + " does not inherit from " + dropRegion.screenEntity.identity.name;
                colour = Color.red;
                statusMessageBox.GetComponent<StatusHandler>().updateStatus(status, colour);
                return false;
            }
            else
            {
                status = "Status: " + "All objects inherit from container type";
                colour = new Color32(33,171,74,255);
                statusMessageBox.GetComponent<StatusHandler>().updateStatus(status, colour);
                return true;
            }
        }
        return false;
        
    }

}
