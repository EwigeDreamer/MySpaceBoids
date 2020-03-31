using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;

public class MainMenuUI : UIBase
{
    public event Action OnPlayPressed = delegate { };
    public event Action OnInfoPressed = delegate { };
    public event Action OnQuitPressed = delegate { };

#pragma warning disable 649
    [SerializeField] Button playBtn;
    [SerializeField] Button infoBtn;
    [SerializeField] Button quitBtn;
#pragma warning restore 649

    void Awake()
    {
        this.playBtn.onClick.AddListener(() => OnPlayPressed());
        this.infoBtn.onClick.AddListener(() => OnInfoPressed());
        this.quitBtn.onClick.AddListener(() => OnQuitPressed());
    }
}