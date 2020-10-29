using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelector : MonoBehaviour
{
    public MaterialUI.SelectionBoxConfig themeDropDown;
    public MaterialUI.SelectionBoxConfig languageDropDown;
    public Button button;
    public GameObject fade;

    private bool themeSet = false;
    private bool languageSet = false;

    // Start is called before the first frame update
    void Awake()
    {
        setDropDowns();
    }

    private void setDropDowns()
    {
        string[] themeArray = randomiseArray(new string[] { Theme.Animals.ToString(), Theme.People.ToString(), Theme.Vehicles.ToString() });
        themeDropDown.listItems = themeArray;
        themeDropDown.ItemPicked += selectTheme;

        string[] languageArray = randomiseArray(new string[] { "C++", "C#", Language.Java.ToString() });
        languageDropDown.listItems = languageArray;
        languageDropDown.ItemPicked += selectLanguage;
    }
    private string[] randomiseArray(string[] array)
    {
        System.Random rnd = new System.Random();
        string[] MyRandomArray = array.OrderBy(x => rnd.Next(1,1000)).ToArray();
        return MyRandomArray;
    }

    public void selectTheme(int id)
    {
        themeSet = true;
        GameManager.theme = (Theme)Enum.Parse(typeof(Theme), themeDropDown.listItems[id]);
        checkSelectionStatus();
    }

    public void selectLanguage(int id)
    {
        languageSet = true;
        string codingLanguage = languageDropDown.listItems[id];
        GameManager.codingLanguage = codingLanguage.Equals("C++") ? Language.CPlusPlus : codingLanguage.Equals("CSharp") ? Language.CSharp : Language.Java;
        checkSelectionStatus();
    }

    private void checkSelectionStatus()
    {
        if(languageSet && themeSet)
        {
            button.interactable = true;
        }
    }

    public void clickContinueButton()
    {
        GameManager.logTheme();
        fade.SetActive(false);
        this.gameObject.SetActive(false);
        GameManager.themeSet = true;
    }
}
