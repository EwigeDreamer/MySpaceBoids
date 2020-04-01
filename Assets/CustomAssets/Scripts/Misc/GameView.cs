using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class GameView : MonoSingleton<GameView>
{
#pragma warning disable 649
    [SerializeField] PlanetController planetController;
    [SerializeField] PlayerController playerController;
#pragma warning restore 649

    public PlanetController PlanetController => this.planetController;
    public PlayerController Player => this.playerController;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.planetController);
        ValidateFind(ref this.playerController);
    }

    public void Init()
    {
        this.planetController.Init();
        this.playerController.Init();
    }
}
