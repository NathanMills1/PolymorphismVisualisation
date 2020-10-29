
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityRepresentation : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Entity representation;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        gameObject.GetComponentInChildren<Text>().gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        this.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameObject clone = Instantiate(this.gameObject);
        clone.GetComponent<EntityRepresentation>().setEntity(this.representation);
        clone.transform.SetParent(this.transform.parent, false);
        clone.transform.SetSiblingIndex(transform.GetSiblingIndex());
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public Entity getRepresentation()
    {
        return representation;
    }

    public void setEntity(Entity entity)
    {
        this.representation = entity;
    }

}
