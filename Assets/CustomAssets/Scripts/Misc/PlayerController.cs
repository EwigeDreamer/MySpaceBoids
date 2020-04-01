using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlayerController : MonoValidate
{
    [SerializeField] BoidController boidController;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.boidController);
    }

    public void Init()
    {
        this.boidController.Init();
    }
}
