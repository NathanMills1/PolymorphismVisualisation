using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Entity
{
    private static List<Identity> parentIdentities;
    private static GameObject screenTemplate;
    private static GameObject objectTemplate;

    public Sprite screenImage;
    public Sprite objectImage;

    public Entity parent { get; set; }
    private List<Entity> children { get; }

    public int height { get; set; }
    public int generation { get; }
    public int id { get;  }
    public string objectColour { get; }
    public Identity identity { get; private set; }

    protected bool gameObjectMade = false;

    public Entity(Entity parent, int id, int height, Sprite image, Sprite objectImage, string objectColour) : this(id, height, image, objectImage, objectColour)
    {
        this.parent = parent;
        parent.addChild(this);
        generation = this.parent.generation + 1;
    }

    public Entity(int id,int height, Sprite image, Sprite objectImage, string objectColour)
    {
        this.id = id;
        this.height = height;
        this.screenImage = image;
        this.objectImage = objectImage;
        this.children = new List<Entity>();
        this.objectColour = objectColour;
        generation = 1;

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

            GameObject objectRepresentation = createObjectRepresentation();
            objectRepresentation.transform.SetParent(objectList.transform);

            gameObjectMade = true;
        }

    }

    public GameObject createObjectRepresentation()
    {
        GameObject objectRepresentation = Object.Instantiate(objectTemplate);
        objectRepresentation.GetComponentInChildren<EntityRepresentation>().setEntity(this);
        objectRepresentation.SetActive(true);
        objectRepresentation.GetComponentInChildren<UnityEngine.UI.Image>().sprite = objectImage;
        objectRepresentation.GetComponent<LayoutElement>().minHeight = height + 10;
        objectRepresentation.GetComponentInChildren<Text>().text = identity.name;
        TextMeshProUGUI[] texts = objectRepresentation.GetComponentsInChildren<TextMeshProUGUI>();
        updateMethodNames(texts);
        
        return objectRepresentation;
    }

    public GameObject createScreenRepresentation()
    {
        GameObject screen = Object.Instantiate(screenTemplate);
        screen.GetComponentInChildren<EntityRepresentation>().setEntity(this);
        screen.SetActive(true);
        screen.GetComponentInChildren<UnityEngine.UI.Image>().sprite = screenImage;
        screen.GetComponent<LayoutElement>().minHeight = height + 10;
        screen.GetComponentInChildren<Text>().text = identity.name;
        return screen;
    }

    public void assignIdentity()
    {
        if (identity == null)
        {
            List<Identity> possibleIdentities = (parent != null) ? parent.identity.children : parentIdentities;

            int identityPos = new System.Random().Next(possibleIdentities.Count);
            identity = possibleIdentities[identityPos];
            possibleIdentities.RemoveAt(identityPos);
        }
    }

    public bool determineIfChildOf(Entity entity)
    {
        if(entity == null)
        {
            return false;
        }

        bool ischild = false;
        Entity currentEntity = this;
        while (!ischild)
        {
            if(currentEntity.Equals(entity)){
                ischild = true;
            } 
            else if(currentEntity.parent != null)
            {
                currentEntity = currentEntity.parent;
            }
            else
            {
                break;
            }
        }
        return ischild;
    }

    public void updateMethodNames(TextMeshProUGUI[] methods)
    {
        foreach (TextMeshProUGUI text in methods)
        {
            
            int pos;
            string[] colours;
            switch (text.name)
            {
                case "Method1":
                case "Method2":
                    pos = int.Parse(text.name.Replace("Method", "")) - 1;
                    Entity parentEntity = this;
                    for (int i = this.generation; i > 1; i--)
                    {
                        parentEntity = parentEntity.parent;
                    }
                    text.text = parentEntity.identity.selectedMethods[pos] + "()";
                    colours = parentEntity.objectColour.Split(',');
                    text.color = new Color32(byte.Parse(colours[0]), byte.Parse(colours[1]), byte.Parse(colours[2]), 255);
                    text.gameObject.SetActive(true);

                    break;
                case "Method3":
                case "Method4":
                case "Method5":
                    if (generation > 1)
                    {
                        pos = int.Parse(text.name.Replace("Method", "")) - 3;
                        Entity childEntity = this;
                        for (int i = this.generation; i > 2; i--)
                        {
                            childEntity = childEntity.parent;
                        }
                        text.text = childEntity.identity.selectedMethods[pos] + "()";
                        colours = childEntity.objectColour.Split(',');
                        text.color = new Color32(byte.Parse(colours[0]), byte.Parse(colours[1]), byte.Parse(colours[2]), 255);
                        text.gameObject.SetActive(true);
                    }
                    else
                    {
                        text.gameObject.SetActive(false);
                    }

                    break;
                case "Method6":
                case "Method7":
                    if (generation > 2)
                    {
                        pos = int.Parse(text.name.Replace("Method", "")) - 6;
                        text.text = identity.selectedMethods[pos] + "()";
                        colours = this.objectColour.Split(',');
                        text.color = new Color32(byte.Parse(colours[0]), byte.Parse(colours[1]), byte.Parse(colours[2]), 255);
                        text.gameObject.SetActive(true);
                    }
                    else
                    {
                        text.gameObject.SetActive(false);
                    }

                    
                    break;
                default:
                    break;
                
            }
        }
    }

    public int getMethodCount()
    {
        int totalMethods = 0;
        Identity currentIdentity = this.identity;
        while(currentIdentity != null)
        {
            totalMethods += currentIdentity.selectedMethods.Count;
            currentIdentity = currentIdentity.parent;
        }

        return totalMethods;
    }

    public bool Equals(Entity other)
    {
        return (other.id.Equals(this.id));
    }
}
