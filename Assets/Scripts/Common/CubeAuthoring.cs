using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
            }
        }
    }

    public struct CubeTag : IComponentData {}
}