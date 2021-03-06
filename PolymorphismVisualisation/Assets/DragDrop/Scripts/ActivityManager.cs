﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityManager : MonoBehaviour
{
    public InheritanceGenerator inheritanceGenerator;
    public TreeManager treeGenerator;
    public QuestionManager questionManager;
    public TutorialManager tutorialManager;
    public DropRegion dropRegion;

    public GameObject pauseMenu;
    public GameObject pauseFade;
    

    public static bool paused = false;

    public void Start()
    {
        loadActivitySection(GameManager.activeActivity, GameManager.theme, GameManager.codingLanguage, GameManager.skipTutorials);
    }

    public void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            paused = togglePause();
        }
    }

    public void OnGUI()
    {
        pauseMenu.SetActive(paused);
        pauseFade.SetActive(paused);
    }

    private void loadActivitySection(int activitySection, Theme theme, Language language, bool skipTutorial)
    {
        inheritanceGenerator.setupEntities(activitySection, theme);
        treeGenerator.loadInheritanceTree(activitySection);
        questionManager.setupQuestionManager(activitySection, language);
        dropRegion.adjustForActivitySection(activitySection);
        tutorialManager.loadTutorial(skipTutorial);
    }

    public static bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}
