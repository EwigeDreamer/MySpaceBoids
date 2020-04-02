using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using TMPro;

public class Planet : MonoValidate
{
    public event System.Action OnRelease = delegate { };

#pragma warning disable 649
    [SerializeField] SelectableObject selectable;
    [SerializeField] TMP_Text label;
    [SerializeField] new SpriteRenderer renderer;

    int id = -1;
    int boidCount = 0;
    float radius = 0.5f;
    Vector3 position = default;
#pragma warning restore 649

    public int Id => this.id;

    public SelectableObject Selectable => this.selectable;

    public Vector3 Position => this.position;

    public float Radius => this.radius;

    public int BoidCount
    {
        get => this.boidCount;
        set
        {
            this.boidCount = value;
            label.text = value.ToString();
        }
    }

    public Color Color { set => this.renderer.color = value; }


    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.selectable);
    }

    public void Init(Vector3 position, float radius, int planetId)
    {
        this.position = position;
        TR.position = position;
        this.radius = radius;
        var scale = radius * 2f;
        TR.localScale = Vector3.one * scale;
        id = planetId;
        BoidCount = 0;
    }

    public void Release()
    {
        OnRelease();
        OnRelease = delegate { };
    }
}
