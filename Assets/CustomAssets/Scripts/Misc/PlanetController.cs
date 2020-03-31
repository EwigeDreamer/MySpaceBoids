using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlanetController : MonoValidate
{
    [SerializeField] PlanetSpawner planetSpawner;

    List<Planet> planets = new List<Planet>();

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.planetSpawner);
    }

    public void Init()
    {
        this.planetSpawner.SpawnPlanets(this.planets);
    }
}
