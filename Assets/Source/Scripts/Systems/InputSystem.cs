using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
public sealed class InputSystem : UpdateSystem
{
    private Filter filter;

    public override void OnAwake()
    {
        this.filter = this.World.Filter.With<InputComponent>().With<MoveComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        foreach (Entity entity in this.filter)
        {
            ref MoveComponent moveComponent = ref entity.GetComponent<MoveComponent>();
                moveComponent.Direction = direction;
        }
    }
}