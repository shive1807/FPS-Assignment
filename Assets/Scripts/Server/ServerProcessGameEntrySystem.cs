using Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Server
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerProcessGameEntrySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MobaPrefabs>();
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<MobaTeamRequest, ReceiveRpcCommandRequest>();
            state.RequireForUpdate(state.GetEntityQuery(builder));
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var championPrefab = SystemAPI.GetSingleton<MobaPrefabs>().Champion;
            
            foreach (var (teamRequest, requestSource, requestEntity)
                     in SystemAPI.Query<RefRW<MobaTeamRequest>, RefRW<ReceiveRpcCommandRequest>>().WithEntityAccess())
            {
                ecb.DestroyEntity(requestEntity);
                ecb.AddComponent<NetworkStreamInGame>(requestSource.ValueRO.SourceConnection);
                
                var clientId = SystemAPI.GetComponent<NetworkId>(requestSource.ValueRO.SourceConnection).Value;
                Debug.Log($" Server is assigning the client Id : {clientId}" + championPrefab);

                {
                    var newChamp = ecb.Instantiate(championPrefab);
                    ecb.SetName(newChamp, "Champion");
                    var spawnPos = new float3(0, 1, 0);
                    var newTransform = LocalTransform.FromPosition(spawnPos);
                    ecb.SetComponent(newChamp, newTransform);   
                    ecb.SetComponent(newChamp, new GhostOwner { NetworkId = clientId });
                    ecb.SetComponent(newChamp, new MobaTeam {Value = teamRequest.ValueRO.Value});
                    ecb.AppendToBuffer(requestSource.ValueRO.SourceConnection, new LinkedEntityGroup{ Value = newChamp});
            
                }
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}  