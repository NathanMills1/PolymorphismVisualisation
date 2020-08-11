using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InheritanceTreeGenerator : MonoBehaviour
{
    public GameObject[] treeMembers;
    public GameObject[] parentMembers;
    private Dictionary<GameObject, List<GameObject>> treeMembersByParent { get; set; }

    void Start()
    {
        
    }

    private void assignEntity(Entity entity, GameObject treeMember)
    {
        int children = entity.children.Count;
        if (children > 0)
        {
            for(int i = 0; i<children; i++)
            {
                assignEntity(entity.children[i], treeMembersByParent[treeMember][i]);
            }
        }

        TextMeshProUGUI[] fields = treeMember.GetComponentsInChildren<TextMeshProUGUI>();
        treeMember.GetComponentInChildren<Image>().sprite = entity.objectImage;
        treeMember.GetComponentInChildren<Text>().text = entity.identity.name;
        entity.updateFields(fields, false);
    }

    public void setupInheritanceTree(Dictionary<int, List<Entity>> entitiesByGeneration)
    {
        treeMembersByParent = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject treeMember in treeMembers)
        {
            treeMembersByParent.Add(treeMember, new List<GameObject>());
            foreach (GameObject childMember in treeMembers)
            {
                if (Regex.IsMatch(childMember.name, treeMember.name + "_\\d"))
                {
                    treeMembersByParent[treeMember].Add(childMember);
                }
            }
        }


        int count = parentMembers.Length;
        for(int i = 0; i<count; i++)
        {
            assignEntity(entitiesByGeneration[1][i], parentMembers[i]);
        }
    }

    public void toggleTree(bool toggle)
    {
        this.gameObject.SetActive(toggle);
    }



}
