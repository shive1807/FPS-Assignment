using Unity.Entities;
using Unity.Mathematics;

namespace Common
{
    public struct CubeMoveData : IComponentData
    {
        public float3 TargetPos;
        public float Speed;
    }
}