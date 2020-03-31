using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class SelectableObject : MonoValidate
{
    public event System.Action OnSelect = delegate { };
    public event System.Action OnDeselect = delegate { };

    [SerializeField] ClickableObject2D clickSensor;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.clickSensor);
    }

    private void Awake()
    {
        this.clickSensor.OnClick += Select;
    }

    public void Select()
    {
        OnSelect();
    }

    public void Deselect()
    {
        OnDeselect();
    }
}
