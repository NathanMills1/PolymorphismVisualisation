
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropRegion : MonoBehaviour, IDropHandler {

    public Image screenImage;
    public Image objectImage;
    public GameObject parentTypeError;
    public GameObject fadeOut;
    public GameObject methodColourPreserver;

    private Entity screenEntity;
    private Entity objectEntity;

    private TextMeshProUGUI[] screenMethods;
    private TextMeshProUGUI[] objectMethods;

    void Start()
    {
        screenMethods = screenImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in screenMethods)
        {
            text.gameObject.SetActive(false);
        }

        objectMethods = objectImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in objectMethods)
        {
            text.gameObject.SetActive(false);
        }
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

            updateMethods();
            setMethodColourPreserverSize();
            determineFadeVisibility();
            checkForErrors();
        }
    }

    public void placeScreen(Entity screenEntity)
    {
        this.screenEntity = screenEntity;
        this.screenImage.sprite = screenEntity.screenImage;
        this.screenImage.gameObject.SetActive(true);

    }

    public void placeObject(Entity objectEntity)
    {
        if (screenEntity != null)
        {
            this.objectEntity = objectEntity;
            this.objectImage.sprite = objectEntity.objectImage;
            this.objectImage.gameObject.SetActive(true);
            objectEntity.updateMethodNames(this.screenMethods);
            objectEntity.updateMethodNames(this.objectMethods);

        }
        else //TODO make message detailing to place screen first
        {

        }
    }

    public void setMethodColourPreserverSize()
    {
        if(objectEntity != null)
        {
            int height = System.Math.Min(objectEntity.height,screenEntity.height);
            height = (int)(height / 0.35) - 65;
            methodColourPreserver.GetComponent<RectTransform>().sizeDelta = new Vector2(220, height);
            methodColourPreserver.SetActive(true);
        }
    }

    public void updateMethods()
    {
        if (objectEntity != null)
        {

            int numberOfMethods = screenEntity.identity.totalMethods;
            for (int i = 0; i < objectMethods.Length; i++)
            {
                bool isVisible = i < numberOfMethods && i<objectEntity.identity.totalMethods;
                screenMethods[i].gameObject.SetActive(isVisible);
            }
        }
    }

    public void determineFadeVisibility()
    {
        bool fadeActive = (screenEntity != null && objectEntity != null) ? true : false;
        this.fadeOut.SetActive(fadeActive);
    }

    public void checkForErrors()
    {
        if(objectEntity != null)
        {
            //set size of method error box to match screen size

            bool screenIsParent = objectEntity.determineIfParent(screenEntity);
            if (!screenIsParent && screenEntity.height > objectEntity.height)
            {
                parentTypeError.GetComponent<RectTransform>().sizeDelta = new Vector2(410, ((screenEntity.height - objectEntity.height) / 0.35f) - 10);
                parentTypeError.transform.localPosition = new Vector3(0, 0.0f - (objectEntity.height/0.35f), 0);
            }
            parentTypeError.SetActive(!screenIsParent && screenEntity.height > objectEntity.height);

            if (!screenIsParent && !screenEntity.determineIfParent(objectEntity))
            {
                //TODO error showing that they don't match up
            }
        }
        
    }

    public void clearSelected()
    {
        this.screenImage.gameObject.SetActive(false);
        this.objectImage.gameObject.SetActive(false);
        this.methodColourPreserver.SetActive(false);
        this.parentTypeError.SetActive(false);
        this.fadeOut.SetActive(false);

        this.screenEntity = null;
        this.objectEntity = null;

        foreach(TextMeshProUGUI method in screenMethods)
        {
            method.gameObject.SetActive(false);
        }
        foreach (TextMeshProUGUI method in objectMethods)
        {
            method.gameObject.SetActive(false);
        }

    }



}
