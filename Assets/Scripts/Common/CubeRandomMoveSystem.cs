// CubeRandomMoveSystem.cs

using Common;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.NetCode;

[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct CubeRandomMoveSystem : ISystem
{
    private Random _random;

    public void OnCreate(ref SystemState state)
    {
        _random = new Random(0xABCDEFu); // deterministic seed
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

            // If close to target, pick a new one
            if (math.distance(currentPos, target) < 0.1f)
            {
                target = new float3(
                    random.NextFloat(-2.5f, 2.5f),
                    0f,
                    random.NextFloat(-2.5f, 2.5f)
                );
                moveData.ValueRW.TargetPos = target;
            }

            // Move toward target
            float3 dir = math.normalize(target - currentPos);
            if (!math.any(math.isnan(dir)))

            {
                currentPos += dir * speed * deltaTime;
                transform.ValueRW.Position = currentPos;
            }
        }

        _random = random;
    }
}