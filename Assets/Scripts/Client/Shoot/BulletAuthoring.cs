using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using BoxCollider = Unity.Physics.SphereCollider;

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
                    timer = 1.5f
                });
                
                var sphereCollider = BoxCollider.Create(new SphereGeometry
                {
                    Center = float3.zero,
                    Radius = 0.5f // sphere radius
                });

                AddComponent(entity, new PhysicsCollider { Value = sphereCollider });
            }
        }
    }
    
    public struct Bullet : IComponentData
    {
        public float timer;
    }
}