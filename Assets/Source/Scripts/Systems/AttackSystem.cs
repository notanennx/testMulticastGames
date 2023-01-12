using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AttackSystem))]
public sealed class AttackSystem : UpdateSystem
{
    public override void OnAwake()
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (Entity attackerEntity in this.World.Filter.With<AttackComponent>())
        {
            // Attempt to attack
            ref AttackComponent attackComponent = ref attackerEntity.GetComponent<AttackComponent>();
            if (Time.time > attackComponent.NextAttack)
            {
                // Add all possible victims to the list
                List<HealthComponent> victimHealths = new List<HealthComponent>(); 
                foreach (Entity victimEntity in this.World.Filter.With<HealthComponent>().With<EnemyMarker>())
                {
                    ref HealthComponent healthComponent = ref victimEntity.GetComponent<HealthComponent>();
                        victimHealths.Add(healthComponent);
                }

                // Damage victims in close proximity
                foreach (HealthComponent targetHealth in GetClosestEnemies(attackComponent.Transform, attackComponent.Range, victimHealths))
                {
                    // Damage
                    Entity damageEntity = this.World.CreateEntity();
                        ref DamageEvent damageEvent = ref damageEntity.AddComponent<DamageEvent>();
                            damageEvent.Damage = attackComponent.DamagePerSecond;
                            damageEvent.VictimEntity = targetHealth.Entity;

                    // Debugging
                    Debug.DrawLine(attackComponent.Transform.position, targetHealth.Transform.position, Color.red, 1f);
                }

                // Cooldown by 1 second
                attackComponent.NextAttack = (Time.time + 1f);
            }
        }
    }

    // Gets closest transforms
    public List<HealthComponent> GetClosestEnemies(Transform inputPoint, float inputDistance, List<HealthComponent> inputHealths)
    {
        // Init
        List<HealthComponent> targetsList = new List<HealthComponent>();

        // Search
        targetsList = inputHealths
        .Where(t => Vector3.Distance(t.Transform.position, inputPoint.transform.position) < inputDistance)
        .OrderBy(t => Vector3.Distance(t.Transform.position, inputPoint.transform.position))
        .Take(3)
        .ToList();

        // Return
        return targetsList;
    }
}