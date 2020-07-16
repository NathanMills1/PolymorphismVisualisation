using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Question
{
    protected Entity variableType { get; set; }
    protected Entity childVariableType { get; set; }
    protected int variableGeneration { get; set; }
    protected int numberOfCodeLines { get; set; }
    protected string codeText { get; set; }
    protected string questionText { get; set; }

    protected static GameObject codeBox;
    protected static GameObject questionTextBox;
    protected static GameObject statusMessageBox;
    protected static GameObject checkButton;
    protected static GameObject yesButton;
    protected static GameObject noButton;
    protected static DropRegion dropRegion;

    public abstract void loadQuestion();

    protected abstract string createQuestionText();

    protected virtual string createCodeText()
    {
        string newCodeText = codeText
            .Replace("VariableType", variableType.identity.name)
            .Replace("VariableName", variableType.identity.name.ToLower());

        string objectName = (dropRegion.objectEntity != null) ?
            dropRegion.objectEntity.identity.name :
            "__________";
        newCodeText = newCodeText.Replace("ObjectType", objectName);

        if(variableType.parent != null)
        {
            newCodeText = newCodeText.Replace("ParentType", variableType.parent.identity.name);
        }

        if(childVariableType != null)
        {
            newCodeText = newCodeText.Replace("ChildType", childVariableType.identity.name);
        }

        newCodeText = performQuestionSpecificCodeSwaps(newCodeText);

        return newCodeText;
    }

    protected virtual string performQuestionSpecificCodeSwaps(string newCodeText)
    {
        return newCodeText;
    }

    protected void setCodeBoxHeight()
    {
        int height = 30 + 30 * numberOfCodeLines;
        codeBox.GetComponent<RectTransform>().sizeDelta = new Vector2(codeBox.GetComponent<RectTransform>().rect.width, height);
    }

    public static void setGameObjects(GameObject codeBox, GameObject questionTextBox, GameObject statusMessageBox, GameObject checkButton, GameObject yesButton, GameObject noButton, DropRegion dropRegion)
    {
        Question.codeBox = codeBox;
        Question.questionTextBox = questionTextBox;
        Question.statusMessageBox = statusMessageBox;
        Question.checkButton = checkButton;
        Question.yesButton = yesButton;
        Question.noButton = noButton;
        Question.dropRegion = dropRegion;
    }

    public bool checkCorrectness()
    {
        TextMeshProUGUI textBox = statusMessageBox.GetComponentInChildren<TextMeshProUGUI>();
        if (dropRegion.screenEntity == null)
        {
            textBox.text = "Status: ";
            textBox.color = Color.black;
        } 
        else if(dropRegion.objectEntity == null)
        {
            textBox.text = "Compiler Error: Null reference to object";
            textBox.color = Color.red;
        }
        else if (!dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity))
        {
            textBox.text = "Compiler Error: " + dropRegion.objectEntity.identity.name + " does not inherit from " + dropRegion.screenEntity.identity.name;
            textBox.color = Color.red;
        }

        return performQuestionSpecificCheck(textBox);
    }

    public virtual bool performQuestionSpecificCheck(TextMeshProUGUI textBox)
    {
        return true;
    }
}
