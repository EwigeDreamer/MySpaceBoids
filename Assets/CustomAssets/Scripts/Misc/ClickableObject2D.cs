using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject2D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler
{
    public event System.Action OnClick = delegate { };

    bool pressed = false;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        this.pressed = true;
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        this.pressed = false;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!this.pressed) return;
        this.pressed = false;
        OnClick();
    }
}
