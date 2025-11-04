using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

namespace Client.Player.Input
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct PlayerInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkStreamInGame>();
            state.RequireForUpdate<PlayerInput>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (RefRW<PlayerInput> playerInput in SystemAPI.Query<RefRW<PlayerInput>>().WithAll<GhostOwnerIsLocal>())
            {
                var input = default(PlayerInput);

                input.Move = float2.zero;
                if (UnityEngine.Input.GetKey(KeyCode.W)) input.Move.y += 1;
                if (UnityEngine.Input.GetKey(KeyCode.S)) input.Move.y -= 1;
                if (UnityEngine.Input.GetKey(KeyCode.A)) input.Move.x -= 1;
                if (UnityEngine.Input.GetKey(KeyCode.D)) input.Move.x += 1;
                
                playerInput.ValueRW.Move = input.Move;

            }
        }
    }
}