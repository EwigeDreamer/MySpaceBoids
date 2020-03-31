using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;
using TMPro;
using UnityEngine.EventSystems;

public class GameUI : UIBase
{
    public event Action OnPauseMenuPressed = delegate { };

#pragma warning disable 649
    [SerializeField] Button pauseMenuBtn;
#pragma warning restore 649

    void Awake()
    {
        this.pauseMenuBtn.onClick.AddListener(() => OnPauseMenuPressed());
    }
}