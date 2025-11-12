using Client.Player.Input;
using Common;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Client.Shoot
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct ShootSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MobaPrefabs>();
            state.RequireForUpdate<NetworkTime>();
        }

        public void OnUpdate(ref SystemState state)
        {
            NetworkTime networkTime = SystemAPI.GetSingleton<NetworkTime>();

            MobaPrefabs mobaPrefabs = SystemAPI.GetSingleton<MobaPrefabs>();
            
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (input, localTransform, ghostOwner) in SystemAPI.Query<RefRO<PlayerInput> , RefRO<LocalTransform>, RefRO<GhostOwner>>().WithAll<Simulate>())
            {
                if(networkTime.IsFirstTimeFullyPredictingTick)
                {
                    if (input.ValueRO.Shoot.IsSet)
                    {
                        if (state.World.IsServer())
                        {
                            Entity bullet = ecb.Instantiate(mobaPrefabs.BulletPrefabEntity);
                            ecb.SetComponent(bullet, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                        }
                    }
                }
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}