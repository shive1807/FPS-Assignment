using Client.Player.Input;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Client.Shoot
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct ShootSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkTime>();
        }

        public void OnUpdate(ref SystemState state)
        {
            NetworkTime networkTime = SystemAPI.GetSingleton<NetworkTime>();
            
            foreach (var input in SystemAPI.Query<RefRO<PlayerInput>>().WithAll<Simulate>())
            {
                if(networkTime.IsFirstTimeFullyPredictingTick)
                {
                    if (input.ValueRO.Shoot.IsSet)
                    {
                        Debug.Log(" Shoot true ! " + state.World);
                    }
                }
            }
        }
    }
}