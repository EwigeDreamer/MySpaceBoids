using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Localizator.Text;
using TMPro;

public class LocalizeTextMeshPro : LocTextBase
{
    [SerializeField] TMP_Text text;
    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.text);
    }
    protected override void OnUpdateLoc(string str)
    {
        this.text.text = str;
    }
}
