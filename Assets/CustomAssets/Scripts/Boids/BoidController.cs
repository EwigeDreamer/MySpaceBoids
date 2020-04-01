using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using MyTools.Extensions.Vectors;
using Random = UnityEngine.Random;
using System.Linq;
using Unity.Collections;
using MyTools.Helpers;

public class BoidController : MonoValidate
{
#pragma warning disable 649
    [SerializeField] BoidParameters parameters;
    [SerializeField] new RenderMesh renderer;
    [SerializeField] Vector3 boidScale = new Vector3(0.5f, 0.5f, 0.5f);

    EntityManager manager = null;
    EntityArchetype archetype = default;
    PlanetsComponent planetsConponent = default;
#pragma warning restore 649

    public void Init()
    {
        this.manager = World.Active.GetOrCreateManager<EntityManager>();
        this.archetype = manager.CreateArchetype(
            typeof(Position),
            typeof(Rotation),
            typeof(Scale),
            typeof(Velocity),
            typeof(Acceleration),
            typeof(PlanetTarget),
            typeof(BoidParamsComponent),
            typeof(PlanetsComponent),
            typeof(RenderMesh),
            typeof(ManagerLinkComponent),
            typeof(NeighborsEntityBuffer));

        var planets = GameView.I.PlanetController.Planets;
        this.planetsConponent = new PlanetsComponent
        {
            Count = planets.Count,
            Positions = new NativeArray<float3>(planets.Select(a => new float3(a.Position.x, a.Position.y, a.Position.z)).ToArray(), Allocator.Persistent),
            Radiuses = new NativeArray<float>(planets.Select(a => a.Radius).ToArray(), Allocator.Persistent)
        };
    }

    public void CreateGroup(int bornPlanetId, int targetPlanetId, int count)
    {
        var archetype = this.archetype;
        var parameters = this.parameters.List;
        var planets = GameView.I.PlanetController.Planets;
        var bornPl = planets[bornPlanetId];
        var targetPl = planets[bornPlanetId];
        var planetsConponent = this.planetsConponent;
        var renderer = this.renderer;

        var manager = this.manager;
        var bornPoint = bornPl.Position;
        var bornRadius = bornPl.Radius;
        var dirToTarget = (targetPl.Position - bornPl.Position).normalized;
        var rotation = quaternion.LookRotationSafe(new float3(0f, 0f, 1f), new float3(dirToTarget.x, dirToTarget.y, 0f));
        var scale = new float3(this.boidScale.x, this.boidScale.y, this.boidScale.z);
        for (int i = 0; i < count; ++i)
        {
            var entity = this.manager.CreateEntity(archetype);
            var pos = bornPoint + (Random.insideUnitCircle * bornRadius).ToV3_xy0();
            manager.SetComponentData(entity, new Position { Value = new float3(pos.x, pos.y, pos.z) });
            manager.SetComponentData(entity, new Rotation { Value = rotation });
            manager.SetComponentData(entity, new Scale { Value = scale });
            manager.SetComponentData(entity, new Velocity { Value = new float3(dirToTarget.x, dirToTarget.y, dirToTarget.z) * parameters.initSpeed });
            manager.SetComponentData(entity, new Acceleration { Value = float3.zero });
            manager.SetComponentData(entity, new PlanetTarget { Index = targetPlanetId });
            manager.SetSharedComponentData(entity, parameters);
            manager.SetSharedComponentData(entity, planetsConponent);
            manager.SetSharedComponentData(entity, renderer);
            manager.SetSharedComponentData(entity, new ManagerLinkComponent { Manager = this });
        }
    }

    [ContextMenu("TEST")]
    void Test()
    {
        CreateGroup(0, 1, 100);
    }

    private void OnDestroy()
    {
        using (var entities = this.manager.GetAllEntities(Allocator.Persistent))
            this.manager.DestroyEntity(entities);
        this.planetsConponent.Positions.Dispose();
        this.planetsConponent.Radiuses.Dispose();
    }
}
