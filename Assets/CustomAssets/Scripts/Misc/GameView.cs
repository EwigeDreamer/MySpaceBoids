using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class GameView : MonoSingleton<GameView>
{
    [SerializeField] PlanetSpawner planetSpawner;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.planetSpawner);
    }

    public void Init()
    {
        this.planetSpawner.Init();
    }
}
