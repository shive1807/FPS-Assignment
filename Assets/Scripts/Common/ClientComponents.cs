using Unity.Entities;

namespace Common
{
    public struct ClientTeamRequest : IComponentData
    {
        public TeamType Value;
    }
}