using Unity.Entities;
using UnityEngine;

namespace Common
{
    public struct CubeSpawner : IComponentData
    {
        public Entity Cube;
    }

    [DisallowMultipleComponent]
    public class CubeSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Cube;

        class Baker : Baker<CubeSpawnerAuthoring>
        {
            public override void Bake(CubeSpawnerAuthoring authoring)
            {
                CubeSpawner component = default;
                component.Cube = GetEntity(authoring.Cube, TransformUsageFlags.Dynamic);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, component);
            }
        }
    }
}