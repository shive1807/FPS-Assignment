// CubeSpawnSystem.cs

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Common
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct CubeSpawnSystem : ISystem
    {
        private bool _spawned;

        public void OnUpdate(ref SystemState state)
        {
            if (_spawned) return;

            foreach (var spawner in SystemAPI.Query<RefRO<CubeSpawner>>())
            {
                var em = state.EntityManager;
                var cube = em.Instantiate(spawner.ValueRO.Cube);

                em.SetComponentData(cube, LocalTransform.FromPosition(float3.zero));
                em.AddComponentData(cube, new CubeMoveData
                {
                    TargetPos = new float3(0, 0, 0),
                    Speed = 1.0f
                });

                _spawned = true;
            }
        }
    }
}