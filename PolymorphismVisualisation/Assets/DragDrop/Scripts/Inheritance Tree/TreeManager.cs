using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public InheritanceTreeGenerator basicTree;
    public InheritanceTreeGenerator inheritanceTree;
    public InheritanceTreeGenerator multiInheritanceTree;

    public void loadInheritanceTree(int activitySection)
    {
        basicTree.gameObject.SetActive(false);
        inheritanceTree.gameObject.SetActive(false);
        multiInheritanceTree.gameObject.SetActive(false);

        InheritanceTreeGenerator treeGenerator;

        switch (activitySection)
        {
            case 1:
                treeGenerator = basicTree;
                break;
            case 2:
                treeGenerator = inheritanceTree;
                break;
            default:
                treeGenerator = multiInheritanceTree;
                break;
        }

        treeGenerator.setupInheritanceTree();
        treeGenerator.gameObject.SetActive(true);
    }

    public void toggleTree(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }
}


