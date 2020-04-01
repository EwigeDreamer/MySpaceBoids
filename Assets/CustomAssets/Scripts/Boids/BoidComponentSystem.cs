using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BoidsSystemGroup { }

[UpdateBefore(typeof(BoidsSystemGroup))]
public class NeighborDetectionSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        [ReadOnly] public ComponentDataArray<Position> positions;
        [ReadOnly] public EntityArray entities;
        public ComponentDataArray<Velocity> velocities;
        [WriteOnly] public BufferArray<NeighborsEntityBuffer> neighbors;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            float prodThresh = math.cos(math.radians(param.neighborFov));
            float distThresh = param.neighborDistance;
            data.neighbors[i].Clear();

            float3 pos0 = data.positions[i].Value;
            float3 fwd0 = math.normalize(data.velocities[i].Value);

            for (int j = 0; j < data.Length; ++j)
            {
                if (i == j) continue;

                float3 pos1 = data.positions[j].Value;
                var to = pos1 - pos0;
                var dist = math.length(to);

                if (dist < distThresh)
                {
                    var dir = math.normalize(to);
                    var prod = Vector3.Dot(dir, fwd0);
                    if (prod > prodThresh)
                    {
                        data.neighbors[i].Add(new NeighborsEntityBuffer { Value = data.entities[j] });
                    }
                }
            }
        }
    }
}

[UpdateInGroup(typeof(BoidsSystemGroup))]
public class ObstacleSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        [ReadOnly] public ComponentDataArray<Position> positions;
        [ReadOnly] public SharedComponentDataArray<PlanetsComponent> planets;
        [ReadOnly] public ComponentDataArray<PlanetTarget> targets;
        public ComponentDataArray<Acceleration> accelerations;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            var thresh = param.obstacleDistance;
            var weight = param.obstacleWeight;
            float3 pos = data.positions[i].Value;
            float3 accel = data.accelerations[i].Value;
            var target = data.targets[i];
            var planets = data.planets[i];
            for (int j = 0; j < planets.Count; ++j)
            {
                if (j == target.Index) continue;
                var point = planets.Positions[j];
                point.z = pos.z;
                var radius = planets.Radiuses[j];
                var fromTo = pos - point;
                var dist = math.length(fromTo);
                var dir = math.normalize(fromTo);
                var norm = math.max((dist - radius) / thresh, 1e-5f);
                if (norm > 1f) continue;
                accel += dir * weight / norm;
            }
            data.accelerations[i] = new Acceleration { Value = accel };
        }
    }
}

[UpdateInGroup(typeof(BoidsSystemGroup))]
public class TargetSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        [ReadOnly] public ComponentDataArray<Position> positions;
        [ReadOnly] public SharedComponentDataArray<PlanetsComponent> planets;
        [ReadOnly] public ComponentDataArray<PlanetTarget> targets;
        public ComponentDataArray<Acceleration> accelerations;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            float3 pos = data.positions[i].Value;
            float3 accel = data.accelerations[i].Value;
            var targetId = data.targets[i].Index;
            var planets = data.planets[i];
            var targetPlanetPos = planets.Positions[targetId];
            var dir = math.normalize(targetPlanetPos - pos);
            var weight = param.targetWeight;
            accel += dir * weight;
            data.accelerations[i] = new Acceleration { Value = accel };
        }
    }
}

[UpdateInGroup(typeof(BoidsSystemGroup))]
public class SeparationSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        [ReadOnly] public ComponentDataArray<Position> positions;
        public ComponentDataArray<Acceleration> accelerations;
        [ReadOnly] public BufferArray<NeighborsEntityBuffer> neighbors;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            var neighbors = data.neighbors[i].Reinterpret<Entity>();
            if (neighbors.Length == 0) continue;

            var force = float3.zero;
            var pos0 = data.positions[i].Value;
            var accel = data.accelerations[i].Value;

            for (int j = 0; j < neighbors.Length; ++j)
            {
                var pos1 = EntityManager.GetComponentData<Position>(neighbors[j]).Value;
                force += math.normalize(pos0 - pos1);
            }

            force /= neighbors.Length;
            var dAccel = force * param.separationWeight;
            data.accelerations[i] = new Acceleration { Value = accel + dAccel };
        }
    }
}


[UpdateInGroup(typeof(BoidsSystemGroup))]
public class AlignmentSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        [ReadOnly] public ComponentDataArray<Velocity> velocities;
        public ComponentDataArray<Acceleration> accelerations;
        [ReadOnly] public BufferArray<NeighborsEntityBuffer> neighbors;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            var neighbors = data.neighbors[i].Reinterpret<Entity>();
            if (neighbors.Length == 0) continue;

            var averageVelocity = float3.zero;
            var velocity = data.velocities[i].Value;
            var accel = data.accelerations[i].Value;

            for (int j = 0; j < neighbors.Length; ++j)
            {
                averageVelocity += EntityManager.GetComponentData<Velocity>(neighbors[j]).Value;
            }

            averageVelocity /= neighbors.Length;
            var dAccel = (averageVelocity - velocity) * param.alignmentWeight;
            data.accelerations[i] = new Acceleration { Value = accel + dAccel };
        }
    }
}

[UpdateInGroup(typeof(BoidsSystemGroup))]
public class CohesionSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        [ReadOnly] public ComponentDataArray<Position> positions;
        public ComponentDataArray<Acceleration> accelerations;
        [ReadOnly] public BufferArray<NeighborsEntityBuffer> neighbors;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            var neighbors = data.neighbors[i].Reinterpret<Entity>();
            if (neighbors.Length == 0) continue;

            var averagePos = float3.zero;
            var pos = data.positions[i].Value;
            var accel = data.accelerations[i].Value;

            for (int j = 0; j < neighbors.Length; ++j)
            {
                averagePos += EntityManager.GetComponentData<Position>(neighbors[j]).Value;
            }

            averagePos /= neighbors.Length;
            var dAccel = (averagePos - pos) * param.cohesionWeight;

            data.accelerations[i] = new Acceleration { Value = accel + dAccel };
        }
    }
}

[UpdateAfter(typeof(BoidsSystemGroup))]
public class MoveSystem : ComponentSystem
{
    struct Data
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public SharedComponentDataArray<BoidParamsComponent> parameters;
        public ComponentDataArray<Position> positions;
        public ComponentDataArray<Rotation> rotations;
        public ComponentDataArray<Velocity> velocities;
        public ComponentDataArray<Acceleration> accelerations;
#pragma warning restore 649
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        var dt = Time.deltaTime;
        for (int i = 0; i < data.Length; ++i)
        {
            var param = data.parameters[i];
            var minSpeed = param.minSpeed;
            var maxSpeed = param.maxSpeed;
            var velocity = data.velocities[i].Value;
            var pos = data.positions[i].Value;
            var accel = data.accelerations[i].Value;

            velocity += accel * dt;
            velocity.z = 0f;
            var dir = math.normalize(velocity);
            var speed = math.length(velocity);
            velocity = math.clamp(speed, minSpeed, maxSpeed) * dir;
            pos += velocity * dt;
            var rot = quaternion.LookRotationSafe(new float3(0f, 0f, 1f), dir);

            data.velocities[i] = new Velocity { Value = velocity };
            data.positions[i] = new Position { Value = pos };
            data.rotations[i] = new Rotation { Value = rot };
            data.accelerations[i] = new Acceleration { Value = float3.zero };
        }
    }
}

