using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Common
{
    [GhostComponent]
    public struct CubeMoveData : IComponentData
    {
        public float3 TargetPos;
        public float Speed;
    }
}