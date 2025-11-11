using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Common
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct CubeRandomMoveSystem : ISystem
    {
        private Random _random;

        public void OnCreate(ref SystemState state)
        {
            _random = new Random(0xABCDEFu);
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var random = _random;

            foreach (var (transform, moveData) in 
                     SystemAPI.Query<RefRW<LocalTransform>, RefRW<CubeMoveData>>())
            {
                float3 currentPos = transform.ValueRO.Position;
                float3 target = moveData.ValueRO.TargetPos;
                float speed = moveData.ValueRO.Speed;

                float3 toTarget = target - currentPos;
                float dist = math.length(toTarget);

                if (dist < 0.1f)
                {
                    target = new float3(
                        random.NextFloat(-5f, 5f),
                        1f,
                        random.NextFloat(-5f, 5f)
                    );
                    moveData.ValueRW.TargetPos = target;
                }
                else
                {
                    if (speed * deltaTime > dist)
                        currentPos = target;
                    else
                        currentPos += math.normalize(toTarget) * speed * deltaTime;

                    currentPos.y = 1f;
                    transform.ValueRW.Position = currentPos;
                }
            }

            _random = random;
        }
    }
}