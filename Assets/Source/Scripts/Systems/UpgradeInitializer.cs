using UniRx;
using TMPro;
using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(UpgradeInitializer))]
public sealed class UpgradeInitializer : Initializer
{
    public PlayerData PlayerData;
    public PlayerConfig PlayerConfig;

    private Entity playerEntity;
    private UpgradeButtonComponent upgradeButtonComponent;
    private Dictionary<EUpgradeType, TMP_Text> textsByTypes = new Dictionary<EUpgradeType, TMP_Text>();
    private Dictionary<EUpgradeType, UpgradeConfig> upgradeConfigsByTypes = new Dictionary<EUpgradeType, UpgradeConfig>();

    public override void OnAwake()
    {
        // Get
        foreach (Entity entity in this.World.Filter.With<AttackComponent>())
            playerEntity = entity;

        // Cards
        RegisterCardTexts();

        // Fill
        SubscribeReactiveProperties();
        FillPlayerData();

        // Button
        foreach (Entity upgradeButtonEntity in this.World.Filter.With<UpgradeButtonComponent>())
        {
            upgradeButtonEntity.GetComponent<UpgradeButtonComponent>().Button.OnClickAsObservable().Subscribe(value => {
                // Dispatch upgrade event
                Entity upgradeEntity = this.World.CreateEntity();
                    upgradeEntity.AddComponent<UpgradeEvent>();

            }).AddTo(PlayerData.Disposables);
        }
    }

    // Registers card texts
    private void RegisterCardTexts()
    {
        // Get
        foreach (Entity upgradeCardEntity in this.World.Filter.With<UpgradeCardComponent>())
        {
            ref UpgradeCardComponent upgradeCardComponent = ref upgradeCardEntity.GetComponent<UpgradeCardComponent>();
                textsByTypes.Add(upgradeCardComponent.Type, upgradeCardComponent.Text);
        }
    }

    // Subscribes all reactive properties
    private void SubscribeReactiveProperties()
    {
        // Range
        PlayerData.Range.Subscribe(value => {
            ref AttackComponent attackComponent = ref playerEntity.GetComponent<AttackComponent>();
                attackComponent.Range = PlayerData.Range.Value;

            textsByTypes[EUpgradeType.Range].text = PlayerData.Range.Value.ToString();
        }).AddTo(PlayerData.Disposables);

        // Speed
        PlayerData.Speed.Subscribe(value => {
            ref MoveComponent moveComponent = ref playerEntity.GetComponent<MoveComponent>();
                moveComponent.Speed = PlayerData.Speed.Value;
                
            textsByTypes[EUpgradeType.Speed].text = PlayerData.Speed.Value.ToString();
        }).AddTo(PlayerData.Disposables);

        // DamagePerSecond
        PlayerData.DamagePerSecond.Subscribe(value => {
            ref AttackComponent attackComponent = ref playerEntity.GetComponent<AttackComponent>();
                attackComponent.DamagePerSecond = PlayerData.DamagePerSecond.Value;
                
            textsByTypes[EUpgradeType.DamagePerSecond].text = PlayerData.DamagePerSecond.Value.ToString();
        }).AddTo(PlayerData.Disposables);

        // Registering in dictionary
        PlayerData.UpgradesDictionary.Add(EUpgradeType.Range, PlayerData.Range);
        PlayerData.UpgradesDictionary.Add(EUpgradeType.Speed, PlayerData.Speed);
        PlayerData.UpgradesDictionary.Add(EUpgradeType.DamagePerSecond, PlayerData.DamagePerSecond);
    }

    // Fills our player's data
    private void FillPlayerData()
    {
        // Fill dictionaries and setup starting values
        foreach (UpgradeConfig upgradeConfig in PlayerConfig.Upgrades)
        {
            upgradeConfigsByTypes.Add(upgradeConfig.Type, upgradeConfig);
            PlayerData.UpgradesDictionary[upgradeConfig.Type].Value = upgradeConfigsByTypes[upgradeConfig.Type].StartValue;
        }
    }
}