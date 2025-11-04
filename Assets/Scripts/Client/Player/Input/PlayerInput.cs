using Unity.Mathematics;
using Unity.NetCode;

namespace Client.Player.Input
{
    public struct PlayerInput : IInputComponentData
    {
        public float2 Move;
        public InputEvent Shoot;
    }
}