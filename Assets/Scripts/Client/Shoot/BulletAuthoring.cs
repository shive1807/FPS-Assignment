using Unity.Entities;
using UnityEngine;

namespace Client.Shoot
{
    public class BulletAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bullet
                {
                    timer = 0.5f
                });
            }
        }
    }
    
    public struct Bullet : IComponentData
    {
        public float timer;
    }
}