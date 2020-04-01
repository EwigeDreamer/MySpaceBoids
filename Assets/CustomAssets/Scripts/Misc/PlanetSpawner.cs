using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Vectors;

public class PlanetSpawner : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] int planetCount = 10;
    [SerializeField] BoxCollider field;
    [SerializeField] Planet planetPrefab;

    [Space]
    [SerializeField] float minPlanetRadius = 1f;
    [SerializeField] float maxPlanetRadius = 2f;

    [Space]
    [SerializeField] float minPartOfDist = 0.1f;
    [SerializeField] float maxPartOfDist = 0.4f;
#pragma warning restore 649

    public void SpawnPlanets(List<Planet> list)
    {
        list.Clear();
        var rect = GetWorldField();
        var points = PointArrayUtility.GetArrayOnRectViaBestCandidate(rect, this.planetCount);
        var container = new GameObject("PLANET_CONTAINER").transform;
        var count = points.Length;
        for (int i = 0; i < count; ++i)
        {
            var point = points[i];
            var closest = points[points.Closest(i)];
            var dist = (closest - point).magnitude;
            var planet = Instantiate(planetPrefab, container);
            planet.name = $"{typeof(Planet).Name} [{i}]";
            var minRadius = Mathf.Max(this.minPlanetRadius, dist * this.minPartOfDist);
            var maxRadius = Mathf.Min(this.maxPlanetRadius, dist * this.maxPartOfDist);
            var radius = Random.Range(minRadius, maxRadius);
            planet.Init(point.ToV3_xy0(), radius, i);
            list.Add(planet);
        }
    }

    Rect GetWorldField()
    {
        var bounds = this.field.bounds;
        var fieldPos = bounds.min.ToV2_xy();
        var fieldSize = (bounds.max - bounds.min).ToV2_xy();
        return new Rect(fieldPos, fieldSize);
    }


}
