using Client.Player.Input;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Client.Player.Movement
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct PlayerMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float moveSpeed = 10f;
            
            foreach (var (input, transform) in SystemAPI.Query<RefRO<PlayerInput>, RefRW<LocalTransform>>().WithAll<Simulate>())
            {
                float3 moveVector = new float3(input.ValueRO.Move.x, 0, input.ValueRO.Move.y);
                transform.ValueRW.Position += moveVector * moveSpeed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}