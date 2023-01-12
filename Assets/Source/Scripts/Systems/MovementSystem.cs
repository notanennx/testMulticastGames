using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
public sealed class MovementSystem : UpdateSystem
{
    private Filter filter;

    public override void OnAwake()
    {
        this.filter = this.World.Filter.With<MoveComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (Entity entity in this.filter)
        {
            ref MoveComponent moveComponent = ref entity.GetComponent<MoveComponent>();
                moveComponent.CharacterController.Move(moveComponent.Speed * moveComponent.Direction * Time.deltaTime);
        } 
    }
}