using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class GameUIBinder : MonoValidate
{
    [SerializeField] GameUI gameUI;
    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.gameUI);
    }

    private void Awake()
    {
        this.gameUI.OnPauseMenuPressed += () => PauseManager.Pause = true;
    }
}
