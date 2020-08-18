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
        loadActivitySection(2, Theme.People, Language.CSharp);
    }

    public void loadActivitySection(int activitySection, Theme theme, Language language)
    {
        inheritanceGenerator.setupEntities(activitySection, theme);
        treeGenerator.loadInheritanceTree(activitySection);
        questionManager.setupQuestionManager(activitySection, language);
        dropRegion.adjustForActivitySection(activitySection);
    }
}
