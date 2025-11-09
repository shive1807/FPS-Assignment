using Unity.Entities;
using UnityEngine;

namespace Common
{
    public class CubeAuthoring : MonoBehaviour
    {
        class Baker : Baker<CubeAuthoring>
        {
            public override void Bake(CubeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                // Add a marker component so systems can find cube entities
                AddComponent<CubeTag>(entity);
            }
        }
    }
    
    public struct CubeTag : IComponentData { }

}

