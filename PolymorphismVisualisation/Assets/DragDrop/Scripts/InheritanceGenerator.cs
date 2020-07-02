using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class InheritanceGenerator : MonoBehaviour
{
    public GameObject screenTemplate;
    public GameObject objectTemplate;
    public GameObject screenList;
    public GameObject objectList;

    public Sprite image_Parent1;
    public Sprite image_Parent2;
    public Sprite image_Parent1Child1;
    public Sprite image_Parent2Child1;
    public Sprite image_Parent1Child1SubChild1;
    public Sprite image_Parent2Child1SubChild1;


    // Start is called before the first frame update
    void Start()
    {
        

        List<Entity> entities = loadEntities();
        List<Identity> parentIdentities = loadIdentities();

        Entity.setTemplates(parentIdentities, screenTemplate, objectTemplate);


        int totalHeight = 0;
        foreach(Entity entity in entities){
            entity.constructGameObject(screenList, null);
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
                temp = new Identity(parent, details[1], methods);
            }
            else //Has no parent
            {
                temp = new Identity(details[1], methods);
                parentIdentities.Add(temp);
            }

            identities.Add(details[0], temp);

        }

        return parentIdentities;
    }

    private List<Entity> loadEntities()
    {
        double SCALE_FACTOR = 0.4;
        Dictionary<string, Entity> entityMap = new Dictionary<string, Entity>();
        List<Entity> entities = new List<Entity>();
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Resources\Images.txt");
        foreach (string line in lines)
        {
            string[] details = line.Split('\t');
            Entity temp;
            Sprite image = Resources.Load<Sprite>(details[3]);

            if (!details[2].Trim().Equals(""))
            {
                Entity parent = entityMap[details[2]];
                temp = new Entity(parent, int.Parse(details[1]), image);
            }
            else //Has no parent
            {
                temp = new Entity(int.Parse(details[1]), image);
            }

            temp.height = (int)(temp.height * SCALE_FACTOR);
            entities.Add(temp);
            entityMap.Add(details[0], temp);
        }

        return entities;
    }

}

public class Entity
{
    private static List<Identity> parentIdentities;
    private static GameObject screenTemplate;
    private static GameObject objectTemplate;

    public Entity parent;
    private List<Entity> children;
    public int height;
    private Sprite image;
    protected Identity identity;

    protected bool gameObjectMade = false;

    public Entity(Entity parent, int height, Sprite image) : this(height, image)
    {
        this.parent = parent;
        parent.addChild(this);
    }

    public Entity(int height, Sprite image)
    {
        this.height = height;
        this.image = image;
        this.children = new List<Entity>();
    }

    public static void setTemplates(List<Identity> identities, GameObject screenTemplate, GameObject objectTemplate)
    {
        Entity.parentIdentities = identities;
        Entity.screenTemplate = screenTemplate;
        Entity.objectTemplate = objectTemplate;

    }

    public void addChild(Entity child)
    {
        children.Add(child);
    }

    public void constructGameObject(GameObject screenList, GameObject objectList)
    {

        if (gameObjectMade == false)
        {
            if (parent != null && parent.gameObjectMade == false)
            {
                parent.constructGameObject(screenList, objectList);
            }

            //Also create one for both Screen and 
            assignIdentity();
            GameObject screen = createScreenRepresentation();
            screen.transform.SetParent(screenList.transform);
            gameObjectMade = true;
        }

    }

    public GameObject createObjectRepresentation()
    {
        return null;
    }

    public GameObject createScreenRepresentation()
    {
        GameObject screen = Object.Instantiate(screenTemplate);
        screen.SetActive(true);
        screen.GetComponentInChildren<Image>().sprite = image;
        screen.GetComponent<LayoutElement>().minHeight = height;
        screen.GetComponentInChildren<Text>().text = identity.name;
        return screen;
    }

    public void assignIdentity()
    {
        if(identity == null)
        {
            List<Identity> possibleIdentities = (parent != null) ? parent.identity.children : parentIdentities;

            int identityPos = new System.Random().Next(possibleIdentities.Count);
            identity = possibleIdentities[identityPos];
            possibleIdentities.RemoveAt(identityPos);
        }
    }
}

public class Identity
{
    public Identity parent;
    public List<Identity> children;
    public string name;
    public string[] methods;
    public int generation;

    public Identity(Identity parent, string name, string[] methods) : this(name, methods)
    {
        this.parent = parent;
        parent.addChild(this);
    }

    public Identity(string name, string[] methods)
    {
        this.name = name;
        this.methods = methods;
        children = new List<Identity>();
    }

    public void addChild(Identity child)
    {
        children.Add(child);
    }


}
