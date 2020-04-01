using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System.Collections.ObjectModel;

public class PlanetController : MonoValidate
{
    [SerializeField] PlanetSpawner planetSpawner;

    [SerializeField] List<Planet> planets = new List<Planet>();

    ReadOnlyCollection<Planet> planetsRO = null;
    public ReadOnlyCollection<Planet> Planets => 
        this.planetsRO ?? (this.planetsRO = new ReadOnlyCollection<Planet>(this.planets));

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
