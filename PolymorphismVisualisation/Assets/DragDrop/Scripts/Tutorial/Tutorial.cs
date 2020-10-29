using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TutorialSection[] tutorialSections;
    public int currentPos { get; private set; } = 0;
    bool tutorialComplete = false;

    public TutorialSection getNextSection()
    {
        currentPos++;
        if (currentPos >= tutorialSections.Length)
        {
            tutorialComplete = true;
            return null;
        }
        else
        {
            return tutorialSections[currentPos];
        }
    }

    public TutorialSection getCurrentSection()
    {
        TutorialSection section = tutorialSections[currentPos];
        return section;
    }
}
