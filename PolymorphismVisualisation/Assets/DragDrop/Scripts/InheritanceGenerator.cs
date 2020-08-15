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
    public InheritanceTreeGenerator treeGenerator;
    public QuestionManager questionManager;
    public int activitySection;

    public Theme theme = Theme.Animals;

    private System.Random randomGen = new System.Random();
    public static Dictionary<int, List<Entity>> selectedEntitiesByGeneration { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
        List<Entity> entities = loadEntities();
        List<Identity> parentIdentities = loadIdentities();

        Entity.setTemplates(parentIdentities, screenTemplate, objectTemplate);

        //TODO select which entities to use by some algorithm
        List<Entity> selectedEntities = selectEntities(entities);
        selectedEntitiesByGeneration = sortEntitiesByGeneration(selectedEntities);
        removeObsoleteEntityReferences(selectedEntities);

        //create gameObject representation for each entity
        float totalHeight = 0;

        foreach(Entity entity in selectedEntities)
        {
            entity.constructGameObject(screenList, objectList);
            totalHeight += 20 + entity.height * Entity.SCALE_FACTOR;
        }

        treeGenerator.setupInheritanceTree(selectedEntitiesByGeneration);

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
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Resources\Classes_" + this.theme + ".txt");
        foreach (string line in lines.Skip(1))
        {
            string[] details = line.Split('\t');
            string fields = details[2].Replace("\"","");
            string methods = details[3].Replace("\"", "");
            Identity temp;

            if (!details[4].Trim().Equals(""))
            {
                Identity parent = identities[details[4]];
                temp = new Identity(parent, details[1], fields, methods);
            }
            else //Has no parent
            {
                temp = new Identity(details[1], fields, methods);
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
                temp = new Entity(parent, int.Parse(details[0]), int.Parse(details[1]), screenImage, objectImage, shadowImage);
            }
            else //Has no parent
            {
                temp = new Entity(int.Parse(details[0]), int.Parse(details[1]), screenImage, objectImage, shadowImage);
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

    private List<Entity> selectEntities(List<Entity> allEntities)
    {
        Dictionary<int, List<Entity>> entitiesByGeneration = sortEntitiesByGeneration(allEntities);
        switch (activitySection)
        {
            case 1:
                //select only gen1 entities
                break;
            case 2:
                //Select gen1 and gen2 entities
                break;
            case 3:
                return selectEntitiesForMultiInheritance(entitiesByGeneration);
            default:
                //Set up case for overriding
                break;
        }
        return null;
    }

    private List<Entity> selectEntitiesForMultiInheritance(Dictionary<int, List<Entity>> entitiesByGeneration)
    {
        List<Entity> chosenEntities = new List<Entity>();
        Entity chosenEntity = null;

        for(int i = 0; i<2; i++)
        {
            int entityPos = randomGen.Next(entitiesByGeneration[3].Count);
            chosenEntity = entitiesByGeneration[3][entityPos];
            while (chosenEntities.Contains(chosenEntity.parent))
            {
                entityPos = randomGen.Next(entitiesByGeneration[3].Count);
                chosenEntity = entitiesByGeneration[3][entityPos];
            }

            chosenEntities.Add(chosenEntity);
            chosenEntities.Add(chosenEntity.parent);
            
        }

        chosenEntities.Add(chosenEntity.parent.parent);

        return chosenEntities;

    }

    private void removeObsoleteEntityReferences(List<Entity> selectedEntities)
    {
        foreach(Entity entity in selectedEntities)
        {
            entity.children.RemoveAll(new System.Predicate<Entity>(x => !selectedEntities.Contains(x)));
        }
    }

}
