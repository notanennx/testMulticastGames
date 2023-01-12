using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/UpgradeConfig", order = 1)]
public class UpgradeConfig : ScriptableObject
{
    [Header("Main")]
    public EUpgradeType Type;

    [Header("Stats")]
    public float Chance;
    public float StartValue;
    public float ValueProgressPerLevel;
}
