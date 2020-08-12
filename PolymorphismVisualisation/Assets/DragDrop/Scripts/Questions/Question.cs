using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Question
{
    protected Entity variableType { get; set; }
    protected Entity objectType { get; set; }
    protected Entity childVariableType { get; set; }
    protected int variableGeneration { get; set; }
    protected int numberOfCodeLines { get; set; }
    protected string codeText { get; set; }
    protected string questionText { get; set; }

    protected int variableCodePosition { get; set; }
    protected int objectCodePosition { get; set; }

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
        string newCodeText = performQuestionSpecificCodeSwaps(codeText);

        if(variableType != null)
        {
            string variableName = camelCase(variableType);

            newCodeText = newCodeText.Replace("VariableType", variableType.identity.name)
                .Replace("VariableName", variableName);

            if (variableType.parent != null)
            {
                newCodeText = newCodeText.Replace("ParentType", variableType.parent.identity.name);
            }
        }
        

        string objectType = (dropRegion.objectEntity != null) ?
            dropRegion.objectEntity.identity.name :
            "__________";
        newCodeText = newCodeText.Replace("ObjectType", objectType);

        

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
        int height = 25 + 35 * numberOfCodeLines;
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
        string status;
        Color colour;
        if (dropRegion.screenEntity == null)
        {
            //#TODO make popup box for no screen
        } 
        else if (dropRegion.objectEntity != null && !dropRegion.objectEntity.determineIfChildOf(dropRegion.screenEntity))
        {
            status = "Compiler Error: " + dropRegion.objectEntity.identity.name + " does not inherit from " + dropRegion.screenEntity.identity.name;
            colour = Color.red;
            statusMessageBox.GetComponent<StatusHandler>().updateStatus(status, colour);
        }

        return performQuestionSpecificCheck();
    }

    public virtual bool checkYesNoAnswer(bool userAnswer)
    {
        return false;
    }

    protected virtual bool performQuestionSpecificCheck()
    {
        return true;
    }

    public virtual void screenPlaced(Entity screenRepresentation) {
        TextAnimation textAnimator = codeBox.GetComponentInChildren<TextAnimation>();
        if (variableCodePosition != -1)
        {
            if (!screenRepresentation.Equals(variableType))
            {
                
                textAnimator.StartCoroutine(textAnimator.shakeText(variableCodePosition));
            }
            else
            {
                textAnimator.StartCoroutine(textAnimator.glowText(variableCodePosition));
            }
        }
    }

    public virtual void objectPlaced(Entity objectRepresentation)
    {
        TextAnimation textAnimator = codeBox.GetComponentInChildren<TextAnimation>();
        if (objectCodePosition != -1)
        {
            if (!objectRepresentation.Equals(objectType))
            {

                textAnimator.StartCoroutine(textAnimator.shakeText(objectCodePosition));
            }
            else
            {
                textAnimator.StartCoroutine(textAnimator.glowText(objectCodePosition));
            }
        }
        setButtonStatus(activateButtonsCondition(objectRepresentation));
    }

    protected string camelCase(Entity entity)
    {
        string input = entity.identity.name;
        return input.ToCharArray()[0].ToString().ToLowerInvariant() + input.Substring(1);
    }

    public void setButtonStatus(bool setInteractable)
    {
        yesButton.GetComponent<Button>().interactable = setInteractable;
        noButton.GetComponent<Button>().interactable = setInteractable;
    }

    protected virtual bool activateButtonsCondition(Entity objectRepresentation)
    {
        if(objectRepresentation != null)
        {
            return true;
        }
        return false;
    }


}
