using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Common
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.LocalSimulation)]
    public partial struct CubeSpawnerSystem : ISystem
    {
        private bool _hasSpawned;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_hasSpawned)
                return;

            var spawnerQuery = SystemAPI.QueryBuilder().WithAll<CubeSpawner>().Build();
            var spawners = spawnerQuery.ToComponentDataArray<CubeSpawner>(Allocator.Temp);
            var entityManager = state.EntityManager;

            var random = new Unity.Mathematics.Random(12345);

            foreach (var spawner in spawners)
            {
                for (int i = 0; i < 100; i++)
                {
                    Entity cube = entityManager.Instantiate(spawner.Cube);

                    var pos = new float3(
                        random.NextFloat(-100f, 100f),
                        random.NextFloat(1f, 1f),
                        random.NextFloat(-100f, 100f)
                    );

                    entityManager.SetComponentData(cube, LocalTransform.FromPosition(pos));
                }
            }

            _hasSpawned = true;
        }
    }
}