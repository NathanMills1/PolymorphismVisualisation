using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCreationQuestion : Question
{
    private bool correctAnswer;
    private Entity selected;

    public CollectionCreationQuestion(string codeText, string questionText, int numberOfCodeLines, Entity selected, Entity child1, Entity child2, int variablePosition, int objectPosition, int object2Position)
    {
        this.questionID = 3;
        this.codeText = codeText;
        this.questionText = questionText;
        this.numberOfCodeLines = numberOfCodeLines;
        this.objectType = child1;
        this.objectType = child1;
        this.object2Type = child2;
        this.selected = selected;

        variableCodePosition = variablePosition;
        objectCodePosition = objectPosition;
        object2CodePosition = object2Position;

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
        return questionText;
    }

    protected override string performQuestionSpecificCodeSwaps(string newCodeText)
    {
        string containerType = dropRegion.screenEntity != null ? dropRegion.screenEntity.identity.name : "______";
        string child1Name = camelCase(objectType);
        string child2Name = camelCase(object2Type);
        return newCodeText.Replace("ContainerType", containerType)
            .Replace("Child1Type", objectType.identity.name).Replace("Child1Name", child1Name)
            .Replace("Child2Type", object2Type.identity.name).Replace("Child2Name", child2Name);
    }

    protected override bool performQuestionSpecificCheck()
    {
        string status = "";
        bool result = false;
        if(dropRegion.screenEntity != null)
        {
            if (!objectType.determineIfChildOf(dropRegion.screenEntity))
            {
                status = "Status: " + objectType.identity.name + " does not inherit from " + dropRegion.screenEntity;
            } 
            else if (!object2Type.determineIfChildOf(dropRegion.screenEntity))
            {
                status = "Status: " + object2Type.identity.name + " does not inherit from " + dropRegion.screenEntity;
            }
            else if(dropRegion.objectEntity == objectType || dropRegion.objectEntity == object2Type)
            {
                status = "Status: " + objectType + " and " + object2Type + " both inherit from " + dropRegion.screenEntity;
                result = true;
            }
            else
            {
                status = "Status: " + dropRegion.objectEntity + " is not one of the instances being added to the container";
            }

            updateStatus(status, result);

        }
        return result;
        
    }

    public override string getExpectedScreenAndObject()
    {
        return selected.identity.name + ",any";
    }

}
