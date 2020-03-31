using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelPopup : PopupBase
{
#pragma warning disable 649
    [SerializeField] GameObject windowWin;
    [SerializeField] GameObject windowFail;
    [SerializeField] Button[] returnBtns;
    [SerializeField] GameObject[] levelStars;
#pragma warning restore 649

    protected override int SortDelta => 0;

    protected override void OnInit()
    {
        base.OnInit();
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
    }

    public void SetWindow(bool isWin)
    {
        this.windowWin.SetActive(isWin);
        this.windowFail.SetActive(!isWin);
    }

    public void SetStars(int value)
    {
        var stars = this.levelStars;
        var count = stars.Length;
        for (int i = 0; i < count; ++i) stars[i].SetActive(i < value);
    }
}
