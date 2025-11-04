using Unity.Entities;

namespace Client.Player.Movement
{
    public struct PlayerMovementData : IComponentData
    {
        public float MoveSpeed;
    }
}