using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[System.Serializable]
public struct BoidParamsComponent : ISharedComponentData
{
    public float initSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float neighborDistance;
    public float neighborFov;
    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;
    public float obstacleDistance;
    public float obstacleWeight;
    public float targetWeight;
}

public struct Velocity : IComponentData
{
    public float3 Value;
}

public struct Acceleration : IComponentData
{
    public float3 Value;
}

public struct PlanetTarget : IComponentData
{
    public int Index;
}

public struct PlanetBorn : IComponentData
{
    public int Index;
}

public struct ManagerLinkComponent : ISharedComponentData
{
    public BoidController Manager;
}

public unsafe struct PlanetsComponent : ISharedComponentData
{
    public int Count;
    public NativeArray<float3> Positions;
    public NativeArray<float> Radiuses;
}

[InternalBufferCapacity(8)]
public unsafe struct NeighborsEntityBuffer : IBufferElementData
{
    public Entity Value;
}