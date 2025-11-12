using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using BoxCollider = Unity.Physics.BoxCollider;

namespace Common
{
    public class CubeAuthoring : MonoBehaviour
    {
        public float Speed = 2f;

        public class Baker : Baker<CubeAuthoring>
        {
            public override void Bake(CubeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new LocalTransform
                {
                    Position = float3.zero,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                AddComponent(entity, new CubeTag());
                
                AddComponent(entity, new CubeMoveData
                {
                    TargetPos = new float3(
                        UnityEngine.Random.Range(-5f, 5f),
                        1f,
                        UnityEngine.Random.Range(-5f, 5f)
                    ),
                    Speed = authoring.Speed
                });
                
                // Add PhysicsCollider using BoxGeometry
                var boxCollider = BoxCollider.Create(new BoxGeometry
                {
                    Center = float3.zero,
                    Size = new float3(1f, 1f, 1f),
                    Orientation = quaternion.identity
                });

                AddComponent(entity, new PhysicsCollider { Value = boxCollider });
            }
        }
    }

    public struct CubeTag : IComponentData {}
}