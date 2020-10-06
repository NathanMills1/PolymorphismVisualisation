using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static int activeActivity { get; set; }
    public static Theme theme { get; set; }
    public static Language codingLanguage { get; set; }
    public static bool muted { get; set; }
    public static float volumeLevel { get; private set; }

    public static bool[] sectionsComplete { get; private set; }
    public static bool[] tutorialsComplete { get; private set; }
    public static int[] sectionProgress { get; private set; }
    private static int[] requiredNumberOfQuestions = new int[] { 5, 8, 8, 10000000 };

    private static bool initialised = false;
    public static bool themeSet = false;
    public static bool progressLoaded = false;
    public static LoggingController loggingController;



    public static void initialiseGameState(GameObject loggingController)
    {
        if (!initialised)
        {
            GameManager.loggingController = loggingController.GetComponent<LoggingController>();
            GameManager.loggingController.SectionsCompletedReq();

            sectionsComplete = new bool[] { true, true, true, true };
            tutorialsComplete = new bool[] { false, false, false };
            sectionProgress = new int[] { 0, 0, 0, 0 };
            volumeLevel = 1;
            muted = false;

            initialised = true;
        }
        

    }

    //as activities start from 1, sectionsComplete[0] will always be true, meaning first activity is always available
    public static void completeSection(int section)
    {
        sectionsComplete[section] = true;
        sectionProgress[activeActivity - 1] = 0;
        Debug.Log("Completed " + section);
        loggingController.CompleteSectionReq(section);
    }

    public static bool updateSectionProgress()
    {
        sectionProgress[activeActivity - 1]++;
        if(sectionProgress[activeActivity - 1] >= requiredNumberOfQuestions[activeActivity - 1])
        {
            completeSection(activeActivity);
            return true;
        }
        return false;
    }

    public static string getSectionProgress()
    {
        if(activeActivity < 4)
        {
            return sectionProgress[activeActivity - 1] + "/" + requiredNumberOfQuestions[activeActivity - 1];
        }
        else
        {
            return sectionProgress[activeActivity - 1].ToString();
        }
        
    }

    public static void updateVolume(float newVolumeLevel)
    {
        AudioListener.volume = newVolumeLevel;
        volumeLevel = newVolumeLevel;
    }

    public static LoggingController getLoggingController()
    {
        return loggingController;
    }

    public static void setActivitiesCompleted(string sectionsCompleted)
    {
        int sections = int.Parse(sectionsCompleted);
        Debug.Log(sections);
        for(int i = 0; i<= sections; i++)
        {
            sectionsComplete[i] = true;
        }
        Debug.Log("Sections completed are: " + sectionsComplete[0].ToString());
        progressLoaded = true;
    }

    public static void logTheme()
    {
        loggingController.ConfigReq(theme.ToString(), codingLanguage.ToString());
    }
}
