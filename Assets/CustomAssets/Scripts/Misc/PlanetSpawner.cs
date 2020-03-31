using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Vectors;
using MyTools.Extensions.Vectors;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] int planetCount = 10;
    [SerializeField] BoxCollider field;
    Vector2[] points = null;

    void Start()
    {
        var rect = GetWorldField();

        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = rect.min;
        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = rect.max;

        this.points = PointArrayUtility.GetArrayOnRectViaBestCandidate(rect, this.planetCount);
        foreach (var point in this.points)
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = point.ToV3_xy0();
    }

    Rect GetWorldField()
    {
        var bounds = this.field.bounds;
        var fieldPos = bounds.min.ToV2_xy();
        var fieldSize = (bounds.max - bounds.min).ToV2_xy();
        return new Rect(fieldPos, fieldSize);
    }


}
