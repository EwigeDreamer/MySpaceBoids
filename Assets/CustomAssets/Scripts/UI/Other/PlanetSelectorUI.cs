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
    public event Action<Vector2> OnBeginDrag = delegate { };
    public event Action<Vector2> OnDrag = delegate { };
    public event Action<Vector2> OnEndDrag = delegate { };

    [SerializeField] SelectorSensor sensor;

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
    }
}