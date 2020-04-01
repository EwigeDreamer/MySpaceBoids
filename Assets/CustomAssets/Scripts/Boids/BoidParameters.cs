using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySpaceBoids/BoidParameters", fileName = "BoidParameters")]
public class BoidParameters : ScriptableObject
{
    [SerializeField] BoidParamsComponent list = new BoidParamsComponent
    {
        initSpeed = 2f,
        minSpeed = 2f,
        maxSpeed = 5f,
        neighborDistance = 1f,
        neighborFov = 90f,
        separationWeight = 5f,
        alignmentWeight = 2f,
        cohesionWeight = 3f,
        obstacleDistance = 1f,
        obstacleWeight = 1f,
        targetWeight = 1f,
    };

    public BoidParamsComponent List => this.list;
}
