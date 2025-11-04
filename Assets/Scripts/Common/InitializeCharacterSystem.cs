using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;

namespace Common
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct InitializeCharacterSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (physicsMass, mobaTeam, newCharacterEntity) in SystemAPI.Query<RefRW<PhysicsMass>, RefRO<MobaTeam>>().WithAny<NewChampTag>().WithEntityAccess())
            {
                physicsMass.ValueRW.InverseInertia[0] = 0;
                physicsMass.ValueRW.InverseInertia[1] = 0;
                physicsMass.ValueRW.InverseInertia[2] = 0;
                
                ecb.SetComponent(newCharacterEntity, new URPMaterialPropertyBaseColor{ Value = new float4(1,0,0,1) });
                ecb.RemoveComponent<NewChampTag>(newCharacterEntity);
            }
            ecb.Playback(state.EntityManager);            
        }
    }
}