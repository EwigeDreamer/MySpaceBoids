using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyTools.Helpers;

public class LevelCell : MonoValidate
{
#pragma warning disable 649
    [SerializeField] TMP_Text levelName;
    [SerializeField] GameObject[] levelStars;
    [SerializeField] Button startBtn;
    [SerializeField] Image img;
    [SerializeField] Sprite SpriteEnabled;
    [SerializeField] Sprite SpriteDisabled;
#pragma warning restore 649

    public TMP_Text Name => this.levelName;
    public Button StartBtn => this.startBtn;

    public void SetActive(bool state)
    {
        this.img.sprite = state ? SpriteEnabled : SpriteDisabled;
    }

    public void SetStars(int value)
    {
        var stars = this.levelStars;
        var count = stars.Length;
        for (int i = 0; i < count; ++i) stars[i].SetActive(i < value);
    }
}
