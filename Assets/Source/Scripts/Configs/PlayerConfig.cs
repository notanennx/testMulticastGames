using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    [Header("Main")]
    public GameObject Prefab;
    public List<UpgradeConfig> Upgrades = new List<UpgradeConfig>();
}
