using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour
{
    public InheritanceGenerator inheritanceGenerator;
    public InheritanceTreeGenerator treeGenerator;
    public QuestionManager questionManager;

    public void Start()
    {
        loadActivitySection(3, Theme.People, Language.CSharp);
    }

    public void loadActivitySection(int activitySection, Theme theme, Language language)
    {
        inheritanceGenerator.setupEntities(activitySection, theme);
        treeGenerator.setupInheritanceTree(InheritanceGenerator.selectedEntitiesByGeneration);
        questionManager.setupQuestionManager(activitySection, language);
    }
}
