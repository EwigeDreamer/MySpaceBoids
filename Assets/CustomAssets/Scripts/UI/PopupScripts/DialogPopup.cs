using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogPopup : PopupBase
{
    public event System.Action OnConfirm = delegate { };
    public event System.Action OnDecline = delegate { };

#pragma warning disable 649
    [SerializeField] TMP_Text label;
    [SerializeField] Button confirmBtn;
    [SerializeField] Button declineBtn;
    [SerializeField] Button[] returnBtns;
#pragma warning restore 649

    protected override int SortDelta => 0;

    protected override void OnInit()
    {
        base.OnInit();
        this.confirmBtn.onClick.AddListener(Confirm);
        this.declineBtn.onClick.AddListener(Decline);
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
    void Decline()
    {
        OnDecline();
        Hide(null);
    }
}
