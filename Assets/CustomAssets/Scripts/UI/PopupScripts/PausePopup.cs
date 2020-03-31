using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausePopup : PopupBase
{

#pragma warning disable 649
    [SerializeField] Button[] returnBtns;
    [SerializeField] Button menuBtn;
#pragma warning restore 649

    protected override int SortDelta => 0;

    protected override void OnInit()
    {
        base.OnInit();
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
        menuBtn.onClick.AddListener(() => { GoToMenu(); Hide(null); });
    }

    protected override void OnRemove()
    {
        base.OnRemove();
        PauseManager.Pause = false;
    }

    void GoToMenu()
    {
        GameManager.StopGame();
    }
}

