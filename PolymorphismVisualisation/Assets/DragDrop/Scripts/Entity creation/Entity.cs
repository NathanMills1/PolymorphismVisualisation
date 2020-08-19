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
    public Sprite shadow;

    public Entity parent { get; set; }
    public List<Entity> children { get; private set; }

    public int height { get; set; }
    public static float SCALE_FACTOR { get; private set; }
    public int generation { get; set; }
    public int id { get;  }
    public string objectColour { get; }
    public Identity identity { get; private set; }

    protected bool gameObjectMade = false;

    public Entity(Entity parent, int id, int height, Sprite image, Sprite objectImage, Sprite shadow, string objectColour) : this(id, height, image, objectImage, shadow, objectColour)
    {
        this.parent = parent;
        parent.addChild(this);
        generation = this.parent.generation + 1;
    }

    public Entity(int id,int height, Sprite image, Sprite objectImage, Sprite shadow, string objectColour)
    {
        this.id = id;
        this.height = height;
        this.screenImage = image;
        this.objectImage = objectImage;
        this.shadow = shadow;
        this.children = new List<Entity>();
        this.objectColour = objectColour.Replace("\"", "");
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

    public void constructGameObject(GameObject screenList, GameObject objectList, int activitySection)
    {
        SCALE_FACTOR = activitySection == 1 ? 0.5f : 0.35f;
        if (gameObjectMade == false)
        {
            if (parent != null && parent.gameObjectMade == false)
            {
                parent.constructGameObject(screenList, objectList, activitySection);
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
        objectRepresentation.GetComponent<LayoutElement>().minHeight = height*SCALE_FACTOR + 10;
        objectRepresentation.GetComponent<LayoutElement>().preferredHeight = height * SCALE_FACTOR + 10;
        objectRepresentation.GetComponentInChildren<Text>().text = identity.name;
        TextMeshProUGUI[] texts = objectRepresentation.GetComponentsInChildren<TextMeshProUGUI>();
        updateFields(texts, false);
        
        return objectRepresentation;
    }

    public GameObject createScreenRepresentation()
    {
        GameObject screen = Object.Instantiate(screenTemplate);
        screen.GetComponentInChildren<EntityRepresentation>().setEntity(this);
        screen.SetActive(true);
        screen.GetComponentInChildren<UnityEngine.UI.Image>().sprite = screenImage;
        screen.GetComponent<LayoutElement>().minHeight = height * SCALE_FACTOR + 10;
        screen.GetComponent<LayoutElement>().preferredHeight = height * SCALE_FACTOR + 10;
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

    public void updateFields(TextMeshProUGUI[] methods, bool includeValue)
    {
        foreach (TextMeshProUGUI text in methods)
        {
            
            int pos;
            string[] colours;
            int generationOfSlot = int.Parse(text.name.Substring(0,1));

            if (generation >= generationOfSlot)
            {
                Entity entity = this;
                for (int i = this.generation; i > generationOfSlot; i--)
                {
                    entity = entity.parent;
                }

                if (text.name.Contains("Field"))
                {

                    text.text = includeValue ? entity.identity.generateFieldValue() : entity.identity.field;
                }
                else //Method spot
                {
                    pos = int.Parse(text.name.Substring(text.name.Length - 1)) - 1;
                    text.text = entity.identity.methods[pos] + "()";
                }

                colours = entity.objectColour.Split(',');
                text.color = new Color32(byte.Parse(colours[0]), byte.Parse(colours[1]), byte.Parse(colours[2]), 255);
                text.gameObject.SetActive(true);
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
    }

    public int getMethodCount()
    {
        int totalMethods = 0;
        Identity currentIdentity = this.identity;
        while(currentIdentity != null)
        {
            totalMethods += currentIdentity.methods.Length;
            currentIdentity = currentIdentity.parent;
        }

        return totalMethods;
    }

    public bool Equals(Entity other)
    {
        if(other == null)
        {
            return false;
        }
        return (other.id.Equals(this.id));
    }
}
