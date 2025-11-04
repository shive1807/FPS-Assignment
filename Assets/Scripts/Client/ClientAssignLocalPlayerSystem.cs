using Common;
using Unity.Entities;
using Unity.NetCode;
using Unity.Collections;

namespace Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    [UpdateInGroup(typeof(GhostSimulationSystemGroup))]
    public partial struct ClientAssignLocalPlayerSystem : ISystem
    {
        private EntityQuery _connectionQuery;

        public void OnCreate(ref SystemState state)
        {
            _connectionQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<NetworkId>(),
                ComponentType.ReadOnly<NetworkStreamInGame>()
            );

            state.RequireForUpdate(_connectionQuery);
        }

        public void OnUpdate(ref SystemState state)
        {
            var connectionEntity = _connectionQuery.GetSingletonEntity();
            var networkId = state.EntityManager.GetComponentData<NetworkId>(connectionEntity);

            // ✅ Use ECB to defer structural changes
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (ghostOwner, entity) in SystemAPI.Query<RefRO<GhostOwner>>().WithEntityAccess())
            {
                if (ghostOwner.ValueRO.NetworkId == networkId.Value)
                {
                    if (!state.EntityManager.HasComponent<LocalPlayerTag>(entity))
                    {
                        ecb.AddComponent<LocalPlayerTag>(entity);
                        // Optional: Debug log
                        // UnityEngine.Debug.Log($"Local player tagged: {entity}");
                    }
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}