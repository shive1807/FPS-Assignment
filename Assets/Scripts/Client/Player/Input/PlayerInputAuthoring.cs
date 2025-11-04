using Unity.Entities;
using UnityEngine;

namespace Client.Player.Input
{
    public class PlayerInputAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerInputAuthoring>
        {
            public override void Bake(PlayerInputAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerInput());
            }
        }
    }
}