using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig", order = 1)]
public class EnemyConfig : ScriptableObject
{
    [Header("Main")]
    public EEnemyType Type;
    public GameObject Prefab;

    [Header("Stats")]
    public int Health;
}
