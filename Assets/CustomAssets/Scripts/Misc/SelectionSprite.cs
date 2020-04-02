using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MyTools.Helpers;

public class SelectionSprite : MonoValidate
{
#pragma warning disable 649
    [SerializeField] SelectableObject selectable;
    [SerializeField] GameObject selectionSprite;
#pragma warning restore 649

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.selectable);
    }

    private void Awake()
    {
        this.selectable.OnSelect += () => this.selectionSprite.SetActive(true);
        this.selectable.OnDeselect += () => this.selectionSprite.SetActive(false);
        this.selectionSprite.SetActive(false);
    }
}
