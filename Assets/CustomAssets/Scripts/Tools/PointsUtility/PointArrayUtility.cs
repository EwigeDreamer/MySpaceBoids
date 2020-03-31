using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Rects;
using System;

public static class PointArrayUtility
{
    public static Vector2[] GetArrayOnRectViaBestCandidate(Rect rect, int pointCount)
    {
        if (pointCount < 1) throw new InvalidOperationException($"{nameof(pointCount)} must be greater than zero!");

        List<Vector2> points = new List<Vector2>(pointCount);
        int candidateCount = 20;
        points.Add(rect.RandomPoint());
        for (int i = 1; i < pointCount; ++i)
        {

            Vector2 point = default;
            float sqrDist = 0f;
            for (int j = 1; j < candidateCount; ++j)
            {
                var candidate = rect.RandomPoint();
                var closest = points[points.Closest(candidate)];
                var sqrDistTmp = (closest - candidate).sqrMagnitude;
                if (sqrDistTmp < sqrDist) continue;
                sqrDist = sqrDistTmp;
                point = candidate;
            }
            points.Add(point);
        }
        return points.ToArray();
    }


    public static int Closest<TColl>(this TColl collection, Vector2 point)
            where TColl : IList<Vector2>
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        int closest = 0;
        float sqrDist = float.PositiveInfinity;
        var i = collection.Count;
        while (i-- > 0)
        {
            var sqrDistTmp = (point - collection[i]).sqrMagnitude;
            if (sqrDistTmp > sqrDist) continue;
            sqrDist = sqrDistTmp;
            closest = i;
        }
        return closest;
    }
    public static int Closest<TColl>(this TColl collection, int index)
            where TColl : IList<Vector2>
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        int closest = 0;
        float sqrDist = float.PositiveInfinity;
        var point = collection[index];
        var i = collection.Count;
        while (i-- > 0)
        {
            if (i == index) continue;
            var sqrDistTmp = (point - collection[i]).sqrMagnitude;
            if (sqrDistTmp > sqrDist) continue;
            sqrDist = sqrDistTmp;
            closest = i;
        }
        return closest;
    }
}
