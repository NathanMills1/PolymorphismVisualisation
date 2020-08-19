using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static int activeActivity { get; set; }
    public static Theme theme { get; set; }
    public static Language codingLanguage { get; set; }
    public static bool volumeOn { get; set; }
    public static float volumeLevel { get; set; }

    public static bool[] sectionsComplete { get; private set; }


    public static void initialiseGameState()
    {
        theme = Theme.Animals;
        codingLanguage = Language.CPlusPlus;

        sectionsComplete = new bool[] { true, true, false, false };
        volumeLevel = 1;
        volumeOn = true;

    }

    public static void completeSection(int section)
    {
        sectionsComplete[section - 1] = true;
    }

}
