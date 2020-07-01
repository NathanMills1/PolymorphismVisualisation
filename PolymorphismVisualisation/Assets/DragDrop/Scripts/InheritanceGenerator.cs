using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class InheritanceGenerator : MonoBehaviour
{
    public GameObject screenTemplate;
    public GameObject objectTemplate;
    public GameObject screenList;

    public Sprite image_Parent1;
    public Sprite image_Parent2;
    public Sprite image_Parent1Child1;
    public Sprite image_Parent2Child1;
    public Sprite image_Parent1Child1SubChild1;
    public Sprite image_Parent2Child1SubChild1;


    // Start is called before the first frame update
    void Start()
    {
        Entity parent1 = new Entity(95, image_Parent1);
        Entity parent1Child1 = new Entity(parent1, 187, image_Parent1Child1);
        Entity parent1Child1SubChild1 = new Entity(parent1Child1, 266, image_Parent1Child1SubChild1);
        Entity parent2 = new Entity(95, image_Parent2);
        Entity parent2Child1 = new Entity(parent2, 187, image_Parent2Child1);
        Entity parent2Child1SubChild1 = new Entity(parent2Child1, 266, image_Parent2Child1SubChild1);

        List<Entity> entities = new List<Entity> { parent1, parent1Child1, parent1Child1SubChild1, parent2, parent2Child1, parent2Child1SubChild1 };

        Identity i1 = new Identity("vehicle", null);
        Identity i1_1 = new Identity(i1, "Car", null);
        Identity i1_1_1 = new Identity(i1_1, "Station Wagon", null);
        Identity i2 = new Identity("Animal", null);
        Identity i2_1 = new Identity(i2, "Mammal", null);
        Identity i2_1_1 = new Identity(i2_1, "Ape", null);

        List<Identity> parentIdentities = new List<Identity> { i1, i2 };

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
