using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static int activitySection { get; private set; }
    public static Theme theme { get; private set; }
    public static Language codingLanguage { get; private set; }
    public static bool volumeOn { get; private set; }
    public static float volumeLevel { get; set; }

    public static bool activity1Complete = false;
    public static bool activity2Complete = false;
    public static bool activity3Complete = false;

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public TMP_Dropdown languageDropDown;
    public TMP_Dropdown themeDropDown;

    public void Start()
    {
        volumeLevel = 1;
        codingLanguage = Language.CPlusPlus;
        theme = Theme.Animals;
    }

    public void swapMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void loadActivitySection(int section)
    {
        activitySection = section;
        SceneManager.LoadScene(1);
    }

    public void setCodeLanguage()
    {
        switch (languageDropDown.value)
        {
            case 0:
                codingLanguage = Language.CPlusPlus;
                break;
            case 1:
                codingLanguage = Language.CSharp;
                break;
            case 2:
                codingLanguage = Language.Java;
                break;
            default:
                codingLanguage = Language.CPlusPlus;
                break;
        }
    }

    public void setTheme()
    {
        switch (themeDropDown.value)
        {
            case 0:
                theme = Theme.Animals;
                break;
            case 1:
                theme = Theme.People;
                break;
            case 2:
                theme = Theme.Vehicles;
                break;
            default:
                theme = Theme.Vehicles;
                break;

        }
    }
}
