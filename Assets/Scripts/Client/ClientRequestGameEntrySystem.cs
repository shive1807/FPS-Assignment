using Common;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct ClientRequestGameEntrySystem : ISystem
    {
        private EntityQuery pendingNetworkIdQuery;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkId>();
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<NetworkId>().WithNone<NetworkStreamInGame>();
            pendingNetworkIdQuery = state.GetEntityQuery(builder);
            
            state.RequireForUpdate(pendingNetworkIdQuery);
            state.RequireForUpdate<ClientTeamRequest>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var requestedTeam = SystemAPI.GetSingleton<ClientTeamRequest>().Value;
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var pendingNetworkIds = pendingNetworkIdQuery.ToEntityArray(Allocator.Temp);
            
            foreach (var pendingNetworkId in pendingNetworkIds)
            {
                ecb.AddComponent<NetworkStreamInGame>(pendingNetworkIds);
                var requestTeamEntity = ecb.CreateEntity();
                
                ecb.AddComponent(requestTeamEntity, new MobaTeamRequest { Value = requestedTeam });
                ecb.AddComponent(requestTeamEntity, new SendRpcCommandRequest { TargetConnection = pendingNetworkId });
            }
            ecb.Playback(state.EntityManager);
        }
    }
}