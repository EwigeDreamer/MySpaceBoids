using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class MenuBinder : MonoValidate
{
    [SerializeField] MainMenuUI main;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.main, true);
    }

    private void Awake()
    {
        this.main.OnPlayPressed += () => GameManager.StartGame();
        this.main.OnInfoPressed += () => PopupManager.OpenPopup<MessagePopup>().SetText("Created by Paul Sammler");
        this.main.OnQuitPressed += () => PopupManager.OpenPopup<QuittingPopup>();
    }
}