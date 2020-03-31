using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartLevelPopup : PopupBase
{
    public event System.Action OnStart = delegate { };

#pragma warning disable 649
    [SerializeField] TMP_Text info;
    [SerializeField] Button startBtn;
    [SerializeField] Button[] returnBtns;
    [SerializeField] GameObject[] levelStars;
#pragma warning restore 649

    protected override int SortDelta => 0;

    protected override void OnInit()
    {
        base.OnInit();
        this.startBtn.onClick.AddListener(StartLevel);
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
    }

    public void SetDescription(string str)
    {
        this.info.text = str;
    }

    public void SetStars(int value)
    {
        var stars = this.levelStars;
        var count = stars.Length;
        for (int i = 0; i < count; ++i) stars[i].SetActive(i < value);
    }


    void StartLevel()
    {
        OnStart();
        Hide(null);
    }
}
