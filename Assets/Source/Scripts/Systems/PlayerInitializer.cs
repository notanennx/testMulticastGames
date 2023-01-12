using UniRx;
using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(PlayerInitializer))]
public sealed class PlayerInitializer : Initializer
{
    public PlayerData PlayerData;
    public PlayerConfig PlayerConfig;

    private Entity cameraEntity;
    private Dictionary<EUpgradeType, UpgradeConfig> upgradeConfigsByTypes = new Dictionary<EUpgradeType, UpgradeConfig>();

    public override void OnAwake()
    {
        // Get camera
        foreach (Entity entity in this.World.Filter.With<CameraComponent>())
            cameraEntity = entity;

        // Player spawning
        foreach (Entity entity in this.World.Filter.With<PlayerspawnComponent>())
        {
            // Point
            ref PlayerspawnComponent playerspawnComponent = ref entity.GetComponent<PlayerspawnComponent>();

            // Instance
            Transform playerTransform = Instantiate(PlayerConfig.Prefab, playerspawnComponent.Transform).transform;

            // Entitize
            Entity playerEntity = this.World.CreateEntity();
                ref InputComponent inputComponent = ref playerEntity.AddComponent<InputComponent>();

                ref MoveComponent moveComponent = ref playerEntity.AddComponent<MoveComponent>();
                    moveComponent.CharacterController = playerTransform.GetComponent<CharacterController>();

                ref AttackComponent attackComponent = ref playerEntity.AddComponent<AttackComponent>();
                    attackComponent.Transform = playerTransform;

            // Update camera
            ref CameraComponent cameraComponent = ref cameraEntity.GetComponent<CameraComponent>();
                cameraComponent.CinemachineVirtualCamera.m_Follow = playerTransform;

            break;
        }
    }
}