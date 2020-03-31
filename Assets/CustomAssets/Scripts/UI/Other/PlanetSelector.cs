using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlanetSelector : MonoValidate
{
    [SerializeField] PlanetSelectorUI selectorUI;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.selectorUI);
    }
}
