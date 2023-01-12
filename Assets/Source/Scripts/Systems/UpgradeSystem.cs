using UniRx;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UpgradeSystem))]
public sealed class UpgradeSystem : UpdateSystem
{
    public PlayerData PlayerData;
    public PlayerConfig PlayerConfig;

    private Dictionary<EUpgradeType, UpgradeConfig> upgradeConfigsByTypes = new Dictionary<EUpgradeType, UpgradeConfig>();

    public override void OnAwake()
    {
        // Fill dictionaries
        foreach (UpgradeConfig upgradeConfig in PlayerConfig.Upgrades)
            upgradeConfigsByTypes.Add(upgradeConfig.Type, upgradeConfig);
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (Entity upgradeEntity in this.World.Filter.With<UpgradeEvent>())
        {
            // Get
            EUpgradeType currentType = GetRandomStat();

            // Add
            PlayerData.UpgradesDictionary[currentType].Value += upgradeConfigsByTypes[currentType].ValueProgressPerLevel;

            // Clean
            upgradeEntity.Dispose();
        }
    }

    // Returns a random upgrade type
    private EUpgradeType GetRandomStat()
    {
        // Setup
        int totalWeight = 0;
        foreach (UpgradeConfig upgradeConfig in PlayerConfig.Upgrades)
            totalWeight += (int)(upgradeConfig.Chance * 100);

        // Lookup
        int randomWeight = UnityEngine.Random.Range(0, totalWeight);
        totalWeight = 0;
        foreach (UpgradeConfig upgradeConfig in PlayerConfig.Upgrades)
        {
            totalWeight += (int)(upgradeConfig.Chance * 100);
            if (totalWeight >= randomWeight)
                return upgradeConfig.Type;
        }

        // Return
        return EUpgradeType.Range;
    }
}