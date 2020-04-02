using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlayerController : MonoValidate
{
    public event System.Action OnAllPlanetsCaptured = delegate { };

    [SerializeField] BoidController boidController;
    [SerializeField] Color playerColor = Color.cyan;

    [SerializeField] int startBoidCount = 50;
    [SerializeField] int maxBoidCount = 500;
    [SerializeField] float boidGenerationSpeed = 5f;

    List<int> capturedPlanetIds = new List<int>();
    List<int> slectedPlanetIds = new List<int>();

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
        FillPlanet(planetId, this.startBoidCount);
        this.boidController.OnHitBoid += FillPlanet;
        StartGeneration();
    }

    public void FillPlanet(int index, int count)
    {
        var planet = GameView.I.PlanetController.Planets[index];
        if (IsOwnPlanet(index))
        {
            planet.BoidCount += count;
        }
        else
        {
            planet.BoidCount -= count;
            if (planet.BoidCount == 0) planet.BoidCount -= 1;
            if (planet.BoidCount < 0)
            {
                planet.BoidCount = Mathf.Abs(planet.BoidCount);
                CapturePlanet(index);
            }
        }
    }

    public void SelectPlanets(List<int> planetIds)
    {
        var planets = GameView.I.PlanetController.Planets;
        this.slectedPlanetIds.Clear();
        this.slectedPlanetIds.AddRange(planetIds);
        this.slectedPlanetIds.RemoveAll(id => !this.capturedPlanetIds.Contains(id));
        foreach (var id in this.capturedPlanetIds) planets[id].Selectable.Deselect();
        foreach (var id in this.slectedPlanetIds) planets[id].Selectable.Select();
    }

    public void SendBoids(int targetId)
    {
        var planets = GameView.I.PlanetController.Planets;
        foreach (var bornId in this.slectedPlanetIds)
        {
            var planet = planets[bornId];
            var count = planet.BoidCount / 2;
            planet.BoidCount -= count;
            this.boidController.CreateGroup(bornId, targetId, count);
        }
    }

    public bool IsOwnPlanet(int index)
    {
        return this.capturedPlanetIds.Contains(index);
    }

    void CapturePlanet(int index)
    {
        var planets = GameView.I.PlanetController.Planets;
        var planet = planets[index];
        planet.Color = this.playerColor;
        planet.Release();
        this.capturedPlanetIds.Add(index);
        planet.OnRelease += () => this.capturedPlanetIds.Remove(index);
        if (this.capturedPlanetIds.Count == planets.Count) OnAllPlanetsCaptured();
    }

    void StartGeneration()
    {
        StartCoroutine(Routine());
        IEnumerator Routine()
        {
            var wait = new WaitForSeconds(1f / this.boidGenerationSpeed);
            var planets = GameView.I.PlanetController.Planets;
            while (true)
            {
                yield return wait;
                foreach (var id in this.capturedPlanetIds) 
                    if (planets[id].BoidCount < this.maxBoidCount) 
                        ++planets[id].BoidCount;
            }
        }
    }
}
