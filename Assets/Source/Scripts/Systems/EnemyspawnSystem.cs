using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EnemyspawnSystem))]
public sealed class EnemyspawnSystem : UpdateSystem
{
    public EnemyConfig[] enemyConfigs;

    private List<Transform> enemySpawnpoints = new List<Transform>();

    public override void OnAwake()
    {
        FillEnemySpawnpoints();
    }

    public override void OnUpdate(float deltaTime)
    {
        // Continious enemy event spawning
        AttemptToSpawnNewRandomEnemy();
    }

    // Fills enemy spawnpoints
    private void FillEnemySpawnpoints()
    {
        // Fill spawns list
        foreach (Entity entity in this.World.Filter.With<EnemyspawnComponent>())
            enemySpawnpoints.Add(entity.GetComponent<EnemyspawnComponent>().Transform);

        // Spawn a few enemies
        for (int i = 0; i < UnityEngine.Random.Range(4, 6); i++)
            SpawnRandomEnemy();
    }

    // Gets us a random spawnpoint
    private Transform GetRandomSpawnpoint()
    {
        return enemySpawnpoints[UnityEngine.Random.Range(0, enemySpawnpoints.Count)];
    }

    // Gets us a random spawnpoint
    private EnemyConfig GetRandomEnemyConfig()
    {
        return enemyConfigs[UnityEngine.Random.Range(0, enemyConfigs.Length)];
    }

    // Spawns a random enemy for us
    private void SpawnRandomEnemy()
    {
        // Get
        Transform spawnpointToUse = GetRandomSpawnpoint();
        EnemyConfig configToUse = GetRandomEnemyConfig();

        // Instance
        Transform enemyTransform = Instantiate(configToUse.Prefab, spawnpointToUse).transform;

        // Entitize
        Entity enemyEntity = this.World.CreateEntity();
            ref EnemyMarker enemyMarker = ref enemyEntity.AddComponent<EnemyMarker>();

            ref HealthComponent healthComponent = ref enemyEntity.AddComponent<HealthComponent>();
                healthComponent.Entity = enemyEntity;
                healthComponent.Transform = enemyTransform;
                healthComponent.HealthAmount = configToUse.Health;
    }

    // Attempts to spawn a new random enemy when event happens
    private void AttemptToSpawnNewRandomEnemy()
    {
        // Fill spawns list
        foreach (Entity spawnEntity in this.World.Filter.With<SpawnenemyEvent>())
        {
            // Create
            SpawnRandomEnemy();

            // Dispose
            spawnEntity.Dispose();
        }
    }
}