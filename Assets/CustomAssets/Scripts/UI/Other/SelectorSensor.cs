using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SelectorSensor : MonoBehaviour, 
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public event Action<Vector2> OnClick = delegate { };
    public event Action<Vector2> OnBeginDrag = delegate { };
    public event Action<Vector2> OnDrag = delegate { };
    public event Action<Vector2> OnEndDrag = delegate { };

    int pointerId = -1;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (this.pointerId > -1) return;
        this.pointerId = eventData.pointerId;
        OnBeginDrag(eventData.position);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointerId) return;
        OnDrag(eventData.position);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointerId) return;
        this.pointerId = -1;
        OnEndDrag(eventData.position);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnClick(eventData.position);
    }
}
