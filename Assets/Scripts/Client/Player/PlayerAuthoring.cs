using Client.Player.Movement;
using Unity.Entities;
using UnityEngine;

namespace Client.Player
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float MoveSpeed = 5f;

        class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<PlayerTag>(entity);
                AddComponent(entity, new PlayerMovementData
                {
                    MoveSpeed = authoring.MoveSpeed
                });
            }
        }
    }
}