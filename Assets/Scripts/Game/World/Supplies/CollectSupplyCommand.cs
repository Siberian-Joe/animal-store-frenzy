using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Supplies
{
    [Serializable]
    public sealed class CollectSupplyCommand : IGameCommand
    {
        public CollectSupplyCommand(EntityId actorId, EntityId supplyPointId)
        {
            ActorId = actorId;
            SupplyPointId = supplyPointId;
        }

        public EntityId ActorId { get; }
        public EntityId SupplyPointId { get; }
    }
}