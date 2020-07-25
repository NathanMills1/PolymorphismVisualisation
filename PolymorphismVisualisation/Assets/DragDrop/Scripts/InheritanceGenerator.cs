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
    public QuestionManager questionManager;

    public static Dictionary<int, List<Entity>> selectedEntitiesByGeneration { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
        List<Entity> entities = loadEntities();
        List<Identity> parentIdentities = loadIdentities();

        Entity.setTemplates(parentIdentities, screenTemplate, objectTemplate);

        //TODO select which entities to use by some algorithm
        List<Entity> selectedEntities = entities;
        selectedEntitiesByGeneration = sortEntitiesByGeneration(selectedEntities);

        //create gameObject representation for each entity
        float totalHeight = 0;
        foreach(Entity entity in selectedEntities)
        {
            entity.constructGameObject(screenList, objectList);
            totalHeight += 20 + entity.height * Entity.SCALE_FACTOR;
        }
        screenList.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalHeight + 10);
        objectList.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalHeight);

        questionManager.generateQuestion();

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
        Dictionary<string, Entity> entityById = new Dictionary<string, Entity>();
        List<Entity> entities = new List<Entity>();
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Resources\Images.txt");
        foreach (string line in lines)
        {
            string[] details = line.Split('\t');
            Entity temp;
            Sprite screenImage = Resources.Load<Sprite>(details[3] + " Screen");
            Sprite objectImage = Resources.Load<Sprite>(details[3] + " Object");
            Sprite shadowImage = Resources.Load<Sprite>(details[3] + " Shadow");

            if (!details[2].Trim().Equals(""))
            {
                Entity parent = entityById[details[2]];
                temp = new Entity(parent, int.Parse(details[0]), int.Parse(details[1]), screenImage, objectImage, shadowImage, details[4]);
            }
            else //Has no parent
            {
                temp = new Entity(int.Parse(details[0]), int.Parse(details[1]), screenImage, objectImage, shadowImage, details[4]);
            }
            entities.Add(temp);
            entityById.Add(details[0], temp);
        }

        return entities;
    }

    private Dictionary<int, List<Entity>> sortEntitiesByGeneration(List<Entity> entities)
    {
        Dictionary<int, List<Entity>> entitiesByGeneration = new Dictionary<int, List<Entity>>();
        entitiesByGeneration.Add(1, new List<Entity>());
        entitiesByGeneration.Add(2, new List<Entity>());
        entitiesByGeneration.Add(3, new List<Entity>());

        foreach(Entity entity in entities)
        {
            entitiesByGeneration[entity.generation].Add(entity);
        }

        return entitiesByGeneration;
    }

}
