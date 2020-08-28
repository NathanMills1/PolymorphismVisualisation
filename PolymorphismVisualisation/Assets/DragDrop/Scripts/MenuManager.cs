using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public TMP_Dropdown languageDropDown;
    public TMP_Dropdown themeDropDown;
    public Button[] activityButtons;
    public Slider volumeSlider;
    public Toggle soundToggle;
    public GameObject fade;
    public GameObject themeSelection;
    public GameObject loggingController;
    public GameObject completionMessage;

    public void Awake()
    {
        
    }

    public void Start()
    {
        GameManager.initialiseGameState(loggingController);

        fade.SetActive(!GameManager.themeSet);
        themeSelection.SetActive(!GameManager.themeSet);

        if (GameManager.sectionsComplete[3])
        {
            fade.SetActive(true);
            completionMessage.SetActive(true);
        }
    }

    public void Update()
    {
        volumeSlider.value = GameManager.volumeLevel;
        soundToggle.isOn = !GameManager.muted;
        setAvailableSections();

    }


    private void setAvailableSections()
    {
        if (GameManager.progressLoaded)
        {
            for (int i = 0; i < activityButtons.Length; i++)
            {
                activityButtons[i].interactable = GameManager.sectionsComplete[i];
            }
        }
        
    }

    public void swapMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void loadActivitySection(int section)
    {
        GameManager.activeActivity = section;
        SceneManager.LoadScene(1);
    }

    public void setCodeLanguage()
    {
        Language codingLanguage;
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
        GameManager.codingLanguage = codingLanguage;
    }

    public void setTheme()
    {
        Theme theme;
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
        GameManager.theme = theme;
    }

    public void toggleSound()
    {
        GameManager.muted = !soundToggle.isOn;
    }

    public void adjustVolume()
    {
        GameManager.updateVolume(volumeSlider.value);
    }

    public void closeMessage()
    {
        completionMessage.SetActive(false);
        fade.SetActive(false);
    }
}
