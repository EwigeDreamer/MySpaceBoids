using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlayerController : MonoValidate
{
    [SerializeField] BoidController boidController;
    [SerializeField] Color playerColor = Color.cyan;

    List<int> capturedPlanetIds = new List<int>();

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.boidController);
    }

    public void Init()
    {
        this.boidController.Init(new BoidControllerInitParam
        {
            color = this.playerColor
        });
        var planetId = GameView.I.PlanetController.GetBottomPlanetId();

    }

    void CapturePlanet(int index)
    {
        var planet = GameView.I.PlanetController.Planets[index];
        planet.Color = playerColor;
        planet.BoidCount = 1;
    }
}
