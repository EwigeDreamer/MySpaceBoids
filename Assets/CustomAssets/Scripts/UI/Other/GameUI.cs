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
    public event Action OnFireTriggerOn = delegate { };
    public event Action OnFireTriggerOff = delegate { };



#pragma warning disable 649
    [SerializeField] Button pauseMenuBtn;
    //[SerializeField] Joystick movement;
    [SerializeField] EventTrigger fireTrigger;

    [SerializeField] GameObject[] lifePoints;
#pragma warning restore 649

    //public Joystick MovementJoystick => movement;

    void Awake()
    {
        this.pauseMenuBtn.onClick.AddListener(() => OnPauseMenuPressed());

        var entryOn = new EventTrigger.Entry();
        entryOn.eventID = EventTriggerType.PointerDown;
        entryOn.callback.AddListener(_ => OnFireTriggerOn());
        this.fireTrigger.triggers.Add(entryOn);

        var entryOff = new EventTrigger.Entry();
        entryOff.eventID = EventTriggerType.PointerUp;
        entryOff.callback.AddListener(_ => OnFireTriggerOff());
        this.fireTrigger.triggers.Add(entryOff);
    }

    public void SetLifePoints(int value)
    {
        var points = this.lifePoints;
        var count = points.Length;
        for (int i = 0; i < count; ++i) points[i].SetActive(i < value);
    }
}