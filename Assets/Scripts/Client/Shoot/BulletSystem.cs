using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Client.Shoot
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct BulletSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            const float moveSpeed = 10f;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (localTransform, bullet, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Bullet>>().WithEntityAccess().WithAll<Simulate>())
            {
                localTransform.ValueRW.Position += new float3(0, 0, 1) * moveSpeed * SystemAPI.Time.DeltaTime;

                if (state.World.IsServer())
                {
                    bullet.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                    if (bullet.ValueRW.timer <= 0)
                    {
                        ecb.DestroyEntity(entity);
                    }
                }
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}