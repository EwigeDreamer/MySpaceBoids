using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SelectorSensor : MonoBehaviour, 
    IPointerDownHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerUpHandler
{
    public event Action<Vector2> OnClick = delegate { };
    public event Action<Vector2> OnBeginDrag = delegate { };
    public event Action<Vector2> OnDrag = delegate { };
    public event Action<Vector2> OnEndDrag = delegate { };

    bool isPointerCatched = false;
    int pointerId = 0;
    bool clickFlag = false;


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (this.isPointerCatched) return;
        this.pointerId = eventData.pointerId;
        this.isPointerCatched = true;
        this.clickFlag = true;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointerId) return;
        OnBeginDrag(eventData.position);
        this.clickFlag = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointerId) return;
        OnDrag(eventData.position);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointerId) return;
        OnEndDrag(eventData.position);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointerId) return;
        this.isPointerCatched = false;
        if (this.clickFlag) OnClick(eventData.position);
        this.clickFlag = false;
    }
}
