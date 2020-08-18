using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour
{
    public InheritanceGenerator inheritanceGenerator;
    public TreeManager treeGenerator;
    public QuestionManager questionManager;
    public DropRegion dropRegion;

    public void Start()
    {
    }

    public void Awake()
    {
        loadActivitySection(MenuManager.activitySection, MenuManager.theme, MenuManager.codingLanguage);
    }

    public void loadActivitySection(int activitySection, Theme theme, Language language)
    {
        inheritanceGenerator.setupEntities(activitySection, theme);
        treeGenerator.loadInheritanceTree(activitySection);
        questionManager.setupQuestionManager(activitySection, language);
        dropRegion.adjustForActivitySection(activitySection);
    }
}
