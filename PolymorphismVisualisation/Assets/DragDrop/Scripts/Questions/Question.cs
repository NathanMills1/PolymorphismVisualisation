using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Question
{
    public int questionID { get; protected set; }
    protected Entity variableType { get; set; }
    protected Entity objectType { get; set; }
    protected Entity object2Type { get; set; }
    protected Entity childVariableType { get; set; }
    protected int variableGeneration { get; set; }
    protected int numberOfCodeLines { get; set; }
    protected string codeText { get; set; }
    protected string questionText { get; set; }

    protected int variableCodePosition { get; set; }
    protected int objectCodePosition { get; set; } = -1;
    protected int object2CodePosition { get; set; } = -1;
    protected bool usesCheckButton { get; set; } = false;

    protected static GameObject codeBox;
    protected static GameObject questionTextBox;
    protected static GameObject statusText;
    protected static GameObject checkButton;
    protected static GameObject yesButton;
    protected static GameObject noButton;
    protected static GameObject continueButton;
    protected static DropRegion dropRegion;
    protected static AudioSource correctSound;
    protected static AudioSource incorrectSound;


    public abstract void loadQuestion();

    protected abstract string createQuestionText();

    protected virtual string createCodeText()
    {
        string newCodeText = performQuestionSpecificCodeSwaps(codeText);

        if (variableType != null)
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



        if (childVariableType != null)
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

    public static void setGameObjects(GameObject codeBox, GameObject questionTextBox, GameObject statusMessageBox, GameObject checkButton, GameObject yesButton, GameObject noButton, GameObject continueButton, DropRegion dropRegion, AudioSource correctSound, AudioSource incorrectSound)
    {
        Question.codeBox = codeBox;
        Question.questionTextBox = questionTextBox;
        Question.statusText = statusMessageBox;
        Question.checkButton = checkButton;
        Question.yesButton = yesButton;
        Question.noButton = noButton;
        Question.continueButton = continueButton;
        Question.dropRegion = dropRegion;
        Question.correctSound = correctSound;
        Question.incorrectSound = incorrectSound;
    }

    public bool checkCorrectness()
    {
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
            if (objectRepresentation.Equals(objectType))
            {
                textAnimator.StartCoroutine(textAnimator.glowText(objectCodePosition));

            } else if (object2Type != null && objectRepresentation.Equals(object2Type))
            {
                textAnimator.StartCoroutine(textAnimator.glowText(object2CodePosition));
            }
            else
            {
                textAnimator.StartCoroutine(textAnimator.shakeText(objectCodePosition));
                if (object2Type != null)
                {
                    textAnimator.StartCoroutine(textAnimator.shakeText(object2CodePosition));
                }
            }
        }

        loadYesNoButtons();
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
        if (usesCheckButton)
        {
            checkButton.SetActive(true);
        }
    }

    protected virtual bool activateButtonsCondition(Entity objectRepresentation)
    {
        if (objectRepresentation != null)
        {
            return true;
        }
        return false;
    }

    protected void updateStatus(string status, bool correct)
    {

        if (!GameManager.muted)
        {
            if (correct)
            {
                correctSound.Play();
            }
            else
            {
                incorrectSound.Play();
            }

        }

        TextMeshProUGUI textBox = statusText.GetComponent<TextMeshProUGUI>();

        textBox.gameObject.SetActive(true);
        textBox.text = status;
        textBox.color = correct ? new Color32(0, 225, 0, 255) : new Color32(255, 0, 0, 255);

        textBox.ForceMeshUpdate();

        if (correct)
        {
            yesButton.SetActive(false);
            noButton.SetActive(false);
            continueButton.SetActive(true);
        }

    }

    public virtual string getExpectedScreenAndObject()
    {
        return null;
    }

    public void loadYesNoButtons()
    {
        if (!usesCheckButton)
        {
            bool isVisible = determineYesNoVisibility();
            yesButton.SetActive(isVisible);
            noButton.SetActive(isVisible);
            
        }
    }

    public virtual bool determineYesNoVisibility()
    {
        if (objectType.Equals(dropRegion.objectEntity) && variableType.Equals(dropRegion.screenEntity))
        {
            return true;
        }

        return false;
    }


}
