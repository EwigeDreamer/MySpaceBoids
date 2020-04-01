using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System.Collections.ObjectModel;

public class PlanetController : MonoValidate
{
    [SerializeField] PlanetSpawner planetSpawner;

    List<Planet> planets = new List<Planet>();

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

    public int GetBottomPlanetId()
    {
        float y = float.PositiveInfinity;
        int index = -1;
        var planets = this.planets;
        int count = planets.Count;
        for (int i = 0; i < count; ++i)
        {
            var planet = planets[i];
            if (planet.Position.y > y) continue;
            y = planet.Position.y;
            index = i;
        }
        return index;
    }
}
