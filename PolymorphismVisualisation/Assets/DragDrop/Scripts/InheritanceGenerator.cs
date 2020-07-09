using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InheritanceGenerator : MonoBehaviour
{
    public GameObject screenTemplate;
    public GameObject objectTemplate;
    public GameObject screenList;
    public GameObject objectList;

    // Start is called before the first frame update
    void Start()
    {
        

        List<Entity> entities = loadEntities();
        List<Identity> parentIdentities = loadIdentities();

        Entity.setTemplates(parentIdentities, screenTemplate, objectTemplate);


        int totalHeight = 0;
        foreach(Entity entity in entities){
            entity.constructGameObject(screenList, objectList);
            totalHeight += 10 + entity.height;
        }

        screenList.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalHeight);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Identity> loadIdentities()
    {
        Dictionary<string, Identity> identities = new Dictionary<string, Identity>();
        List<Identity> parentIdentities = new List<Identity>();
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Resources\Classes.txt");
        foreach (string line in lines)
        {
            string[] details = line.Split('\t');
            string[] methods = details[2].Replace("\"", "").Split(',');
            Identity temp;

            if (!details[3].Trim().Equals(""))
            {
                Identity parent = identities[details[3]];
                temp = new Identity(parent, details[1], methods, int.Parse(details[4]));
            }
            else //Has no parent
            {
                temp = new Identity(details[1], methods, int.Parse(details[4]));
                parentIdentities.Add(temp);
            }

            identities.Add(details[0], temp);

        }

        return parentIdentities;
    }

    private List<Entity> loadEntities()
    {
        double SCALE_FACTOR = 0.35;
        Dictionary<string, Entity> entityMap = new Dictionary<string, Entity>();
        List<Entity> entities = new List<Entity>();
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Resources\Images.txt");
        foreach (string line in lines)
        {
            string[] details = line.Split('\t');
            Entity temp;
            Sprite screenImage = Resources.Load<Sprite>(details[3]);
            Sprite objectImage = Resources.Load<Sprite>(details[3] + " Object");

            if (!details[2].Trim().Equals(""))
            {
                Entity parent = entityMap[details[2]];
                temp = new Entity(parent, int.Parse(details[0]), int.Parse(details[1]), screenImage, objectImage, details[4]);
            }
            else //Has no parent
            {
                temp = new Entity(int.Parse(details[0]), int.Parse(details[1]), screenImage, objectImage, details[4]);
            }

            temp.height = (int)(temp.height * SCALE_FACTOR);
            entities.Add(temp);
            entityMap.Add(details[0], temp);
        }

        return entities;
    }

}
