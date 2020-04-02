using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlanetSelector : MonoValidate
{
    [SerializeField] PlanetSelectorUI selectorUI;

    List<int> planetsTmp = new List<int>();

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.selectorUI);
    }

    private void Awake()
    {
        this.selectorUI.OnSquareSelect += Select;
        this.selectorUI.OnClick += Attack;
    }


    void Select(Vector2 corner1, Vector2 corner2)
    {
        this.planetsTmp.Clear();
        corner1 = MainCamera.Camera.ScreenToWorldPoint(corner1);
        corner2 = MainCamera.Camera.ScreenToWorldPoint(corner2);
        var cols = Physics2D.OverlapAreaAll(corner1, corner2);
        foreach (var col in cols)
        {
            var planet = col.GetComponent<Planet>();
            if (planet == null) continue;
            this.planetsTmp.Add(planet.Id);
        }
        GameView.I.Player.SelectPlanets(this.planetsTmp);
    }

    void Attack(Vector2 pos)
    {
        pos = MainCamera.Camera.ScreenToWorldPoint(pos);
        var col = Physics2D.OverlapPoint(pos);
        if (col == null) return;
        var planet = col.GetComponent<Planet>();
        if (planet == null) return;
        GameView.I.Player.SendBoids(planet.Id);
    }
}
