
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropRegion : MonoBehaviour, IDropHandler {

    public Image _screenImage;
    public Image _objectImage;

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            if (eventData.pointerDrag.tag.Equals("Screen"))
            {
                _screenImage.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
            }
        }
    }

}
