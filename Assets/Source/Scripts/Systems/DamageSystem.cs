using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageSystem))]
public sealed class DamageSystem : UpdateSystem
{
    public override void OnAwake()
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (Entity damageEntity in this.World.Filter.With<DamageEvent>())
        {
            // Get
            ref DamageEvent damageEvent = ref damageEntity.GetComponent<DamageEvent>();
                ref Entity victimEntity = ref damageEvent.VictimEntity;
                    ref HealthComponent victimHealth = ref victimEntity.GetComponent<HealthComponent>();

                    // Damage
                    victimHealth.HealthAmount -= damageEvent.Damage;

                    // Destroy
                    if (victimHealth.HealthAmount <= 0)
                    {
                        Destroy(victimHealth.Transform.gameObject);//.SetActive(false);
                        victimHealth.Entity.Dispose();

                        // Spawn new one
                        Entity spawnEntity = this.World.CreateEntity();
                            ref SpawnenemyEvent SpawnenemyEvent = ref spawnEntity.AddComponent<SpawnenemyEvent>();
                    }

            // Clean
            damageEntity.Dispose();
        }
    }
}