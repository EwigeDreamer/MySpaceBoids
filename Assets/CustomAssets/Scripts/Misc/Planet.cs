using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class Planet : MonoValidate
{

    [SerializeField] SelectableObject selectable;

    public Vector3 Position { get; private set; } = default;
    public float Radius { get; private set; } = 0.5f;

    public SelectableObject Selectable => this.selectable;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.selectable);
    }

    public void SetPosition(Vector3 position)
    {
        TR.position = position;
        Position = position;
    }

    public void SetRadius(float radius)
    {
        var scale = radius * 2f;
        TR.localScale = Vector3.one * scale;
        Radius = radius;
    }
}
