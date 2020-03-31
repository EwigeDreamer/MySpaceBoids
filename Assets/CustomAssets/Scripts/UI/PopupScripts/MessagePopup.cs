using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePopup : PopupBase
{
    public event System.Action OnConfirm = delegate { };

#pragma warning disable 649
    [SerializeField] TMP_Text label;
    [SerializeField] Button confirmBtn;
    [SerializeField] Button[] returnBtns;
#pragma warning restore 649

    protected override int SortDelta => 0;

    protected override void OnInit()
    {
        base.OnInit();
        this.confirmBtn.onClick.AddListener(Confirm);
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
    }

    public void SetText(string str)
    {
        this.label.text = str;
    }

    void Confirm()
    {
        OnConfirm();
        Hide(null);
    }
}
