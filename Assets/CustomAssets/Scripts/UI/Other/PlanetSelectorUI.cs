using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;
using TMPro;
using UnityEngine.EventSystems;

public class PlanetSelectorUI : UIBase
{
    public event Action<Vector2> OnClick = delegate { };
    public event Action<Vector2, Vector2> OnSquareSelect = delegate { };

#pragma warning disable 649
    [SerializeField] SelectorSensor sensor;
    [SerializeField] RectTransform square;
#pragma warning restore 649

    Vector2 start = default;
    Vector2 end = default;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponentInChildren(ref this.sensor);
    }

    private void Awake()
    {
        this.sensor.OnClick += pos => OnClick(pos);
        this.sensor.OnBeginDrag += pos => OnBeginDrag(pos);
        this.sensor.OnDrag += pos => OnDrag(pos);
        this.sensor.OnEndDrag += pos => OnEndDrag(pos);
        this.square.gameObject.SetActive(false);
    }

    void OnBeginDrag(Vector2 pos)
    {
        this.start = pos;
        this.square.gameObject.SetActive(true);
    }
    void OnDrag(Vector2 pos)
    {
        PlaceSquare(this.start, pos);
    }

    void OnEndDrag(Vector2 pos)
    {
        this.end = pos;
        this.square.gameObject.SetActive(false);
        OnSquareSelect(this.start, this.end);
    }

    void PlaceSquare(Vector2 corner1, Vector2 corner2)
    {
        var center = (corner1 + corner2) / 2f;
        var size = corner1 - corner2;
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        this.square.anchoredPosition = center;
        this.square.sizeDelta = size;
    }
}