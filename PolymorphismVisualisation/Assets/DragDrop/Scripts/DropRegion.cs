
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropRegion : MonoBehaviour, IDropHandler {

    public Image screenImage;
    public Image bottomScreenImage;
    public Image objectImage;
    public Image screenShadow;
    public GameObject parentTypeError;
    public QuestionManager questionManager;
    public Vector2 screenStartPosition;
    public AudioSource thudSound;

    public Entity screenEntity { get; private set; }
    public Entity objectEntity { get; private set; }

    private TextMeshProUGUI[] objectMethods;
    private List<TextMeshProUGUI> screenNames = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> fieldNames = new List<TextMeshProUGUI>();
    private Image[] shades;


    void Start()
    {
        objectMethods = objectImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in objectMethods)
        {
            text.gameObject.SetActive(false);
        }
        TextMeshProUGUI[] labels = screenImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI label in labels)
        {
            if (label.gameObject.name.Contains("Field"))
            {
                fieldNames.Add(label);
            }
            else
            {
                screenNames.Add(label);
            }
            label.gameObject.SetActive(false);
        }
        shades = screenImage.gameObject.GetComponentsInChildren<Image>();
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            if (eventData.pointerDrag.tag.Equals("Screen"))
            {
                placeScreen(eventData.pointerDrag.GetComponent<EntityRepresentation>().getRepresentation());
            }
            else if (eventData.pointerDrag.tag.Equals("Object"))
            {
                placeObject(eventData.pointerDrag.GetComponent<EntityRepresentation>().getRepresentation());
            }

            questionManager.updateQuestion();
        }
    }

    public void placeScreen(Entity screenEntity)
    {
        if(objectEntity == null)
        {
            this.screenEntity = screenEntity;
            this.screenImage.sprite = screenEntity.screenImage;
            this.screenImage.gameObject.SetActive(true);

            this.bottomScreenImage.sprite = screenEntity.screenImage;
            this.bottomScreenImage.gameObject.SetActive(true);
            
            this.screenShadow.sprite = screenEntity.shadow;
            this.screenShadow.gameObject.SetActive(true);
            updateScreenLabels(screenEntity);
            StartCoroutine(dropScreen());
        }
        else //TODO have error appear stating cannot change screen type while object is placed
        {

        }
        
        
    }

    public void placeObject(Entity objectEntity)
    {
        parentTypeError.SetActive(false);

        if (screenEntity != null)
        {
            this.objectEntity = objectEntity;
            this.objectImage.sprite = objectEntity.objectImage;
            this.objectImage.gameObject.SetActive(true);
            objectEntity.updateFields(this.objectMethods, true);
            StartCoroutine(slideObjectIn());

        }
        else //TODO make message detailing to place screen first
        {

        }
    }

    public void checkForErrors()
    {
        if(objectEntity != null)
        {
            //set size of method error box to match screen size

            bool screenIsParent = objectEntity.determineIfChildOf(screenEntity);
            if (!screenIsParent && screenEntity.height > objectEntity.height)
            {
                parentTypeError.GetComponent<RectTransform>().sizeDelta = new Vector2(410, (screenEntity.height - objectEntity.height));
                parentTypeError.transform.localPosition = new Vector3(0, -100.0f - (objectEntity.height), 0);
            }
            parentTypeError.SetActive(!screenIsParent && screenEntity.height > objectEntity.height);

            if (!screenIsParent && !screenEntity.determineIfChildOf(objectEntity))
            {
                //TODO error showing that they don't match up
            }
        }
        
    }

    private void updateScreenLabels(Entity screen)
    {
        for(int i = 0; i < 3; i++)
        {
            screenNames[i].gameObject.SetActive(screen.generation > i);
            fieldNames[i].gameObject.SetActive(screen.generation > i);
            shades[i].gameObject.SetActive(screen.generation > i);

            Entity currentScreen = screen;
            while (currentScreen.generation > i+1)
            {
                currentScreen = currentScreen.parent;
            }
            screenNames[i].text = currentScreen.identity.name;
            fieldNames[i].text = currentScreen.identity.field;
        }
        
    }

    public void clearSelected()
    {
        this.screenImage.gameObject.SetActive(false);
        this.bottomScreenImage.gameObject.SetActive(false);
        this.objectImage.gameObject.SetActive(false);
        this.parentTypeError.SetActive(false);
        this.screenShadow.gameObject.SetActive(false);

        this.screenEntity = null;
        this.objectEntity = null;

        foreach (TextMeshProUGUI method in objectMethods)
        {
            method.gameObject.SetActive(false);
        }

        questionManager.updateQuestion();
    }

    public IEnumerator dropScreen()
    {
        const float DURATION = 0.4f;
        const float STEP_SIZE = 0.05f;
        Vector2 screenEndPosition = new Vector2(-5, -125);
        float currentTime = 0;

        GameObject screen = screenImage.gameObject;
        GameObject bottomScreen = bottomScreenImage.gameObject;

        while(currentTime <= DURATION)
        {
            float alpha = currentTime / DURATION;
            screen.transform.localPosition = (1 - alpha) * screenStartPosition + alpha * screenEndPosition;
            bottomScreen.transform.localPosition = (1 - alpha) * screenStartPosition + alpha * screenEndPosition - new Vector2(-9,9);
            screenShadow.color = new Color(55, 55, 55, 0.6f * alpha);

            currentTime += STEP_SIZE;

            yield return new WaitForSeconds(STEP_SIZE);
        }

        screen.transform.localPosition = screenEndPosition;
        bottomScreen.transform.localPosition = screenEndPosition - new Vector2(-9, 9);
        thudSound.Play();
        questionManager.screenPlaced(screenEntity);
    }

    public IEnumerator slideObjectIn()
    {
        const float DURATION = 0.6f;
        const float STEP_SIZE = 0.05f;
        Vector2 objectEndPosition = new Vector2(3, -133);
        Vector2 objectStartPosition = new Vector2(400, -133);
        float currentTime = 0;

        GameObject page = objectImage.gameObject;

        while (currentTime <= DURATION)
        {
            float alpha = currentTime / DURATION;
            page.transform.localPosition = (1 - alpha) * objectStartPosition + alpha * objectEndPosition;

            currentTime += STEP_SIZE;

            yield return new WaitForSeconds(STEP_SIZE);
        }

        page.transform.localPosition = objectEndPosition;
        checkForErrors();
        questionManager.objectPlaced(objectEntity);
    }



}
