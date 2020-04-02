using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class SelectableObject : MonoValidate
{
    public event System.Action OnSelect = delegate { };
    public event System.Action OnDeselect = delegate { };

    public void Select()
    {
        OnSelect();
    }

    public void Deselect()
    {
        OnDeselect();
    }
}
