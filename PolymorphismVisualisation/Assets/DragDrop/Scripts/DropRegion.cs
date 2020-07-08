
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropRegion : MonoBehaviour, IDropHandler {

    public Image screenImage;
    public Image objectImage;
    public GameObject ParentTypeError;

    private Entity screenEntity;
    private Entity objectEntity;

    private TextMeshProUGUI[] methods;

    void Start()
    {
        methods = screenImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in methods)
        {
            text.gameObject.SetActive(false);
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            if (eventData.pointerDrag.tag.Equals("Screen"))
            {
                this.screenEntity = eventData.pointerDrag.GetComponent<EntityRepresentation>().getRepresentation();
                this.screenImage.sprite = screenEntity.screenImage;
                this.screenImage.gameObject.SetActive(true);
                ParentTypeError.GetComponent<RectTransform>().sizeDelta = new Vector2(410, (int)(screenEntity.height / 0.35) - 110);

            }
            else if (eventData.pointerDrag.tag.Equals("Object"))
            {
                if (screenEntity != null)
                {

                    this.objectEntity = eventData.pointerDrag.GetComponent<EntityRepresentation>().getRepresentation();
                    this.objectImage.sprite = objectEntity.objectImage;
                    this.objectImage.gameObject.SetActive(true);
                    objectEntity.updateMethodNames(this.methods);
                }
                else //TODO make message detailing to place screen first
                {

                }
            }
            checkForErrors();
        }
    }

    public void checkForErrors()
    {
        if(objectEntity != null)
        {
            bool screenIsParent = objectEntity.determineIfParent(screenEntity);
            ParentTypeError.SetActive(!screenIsParent);

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
        ParentTypeError.SetActive(false);
        this.screenEntity = null;
        this.objectEntity = null;
        foreach(TextMeshProUGUI method in methods)
        {
            method.gameObject.SetActive(false);
        }
    }



}
