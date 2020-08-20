using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Tutorial[] tutorials;
    public DialogueManager dialogueManager;
    public GameObject focusedObjectHolder;
    public GameObject tutorialFade;
    public GameObject tutorialBox;

    private Tutorial currentTutorial;
    private bool tutorialComplete = false;

    public void loadTutorial()
    {
        if(GameManager.activeActivity <= 3 && !GameManager.tutorialsComplete[GameManager.activeActivity - 1])
        {
            currentTutorial = tutorials[GameManager.activeActivity - 1];
            beginTutorial();
        }
        else
        {
            //instead load question as normal
            tutorialComplete = true;
            FindObjectOfType<QuestionManager>().generateQuestion();
        }
    }

    private void beginTutorial()
    {
        tutorialFade.SetActive(true);
        tutorialBox.SetActive(true);
        TutorialSection section = currentTutorial.getCurrentSection();
        dialogueManager.StartDialogue(section.dialogue, section.nextSectionOnDialogueComplete);
        highlightObjects(section);

    }

    public void actionMade(int tutorialPart)
    {
        if(!tutorialComplete && currentTutorial.currentPos.Equals(tutorialPart - 1))
        {
            loadNextSection();
        }
    }

    public void loadNextSection()
    {
        resetObjectPositions();

        TutorialSection section = currentTutorial.getNextSection();
        if(section == null)
        {
            completeTutorial();
        }
        else
        {
            highlightObjects(section);
            dialogueManager.StartDialogue(section.dialogue, section.nextSectionOnDialogueComplete);
        }

        determineQuestionLoad();
    }

    private void determineQuestionLoad()
    {
        switch (GameManager.activeActivity)
        {
            case 1:
                if(currentTutorial.currentPos == 9){
                    FindObjectOfType<QuestionManager>().generateQuestion();
                }
                break;
            default:
                break;
        }
    }

    private void highlightObjects(TutorialSection section)
    {
        for (int i = 0; i < section.highlightedComponents.Length; i++)
        {
            GameObject currentObject = section.highlightedComponents[i];
            int objectPosition = currentObject.transform.GetSiblingIndex();
            section.originalPositions[i] = objectPosition;
            section.componentParents[i] = currentObject.transform.parent;
            currentObject.transform.SetParent(focusedObjectHolder.transform);

        }
    }

    //Sets them back to their original positions in the object hierarchy (Behind the fade)
    private void resetObjectPositions()
    {
        TutorialSection section = currentTutorial.getCurrentSection();
        for(int i = section.highlightedComponents.Length - 1; i>= 0; i--)
        {
            Transform currentObject = section.highlightedComponents[i].transform;
            currentObject.SetParent(section.componentParents[i]);
            currentObject.SetSiblingIndex(section.originalPositions[i]);
        }
    }

    private void completeTutorial()
    {
        tutorialComplete = true;
        tutorialFade.SetActive(false);
        tutorialBox.SetActive(false);
        GameManager.tutorialsComplete[GameManager.activeActivity - 1] = true;
        FindObjectOfType<DropRegion>().clearSelected();
    }
}
