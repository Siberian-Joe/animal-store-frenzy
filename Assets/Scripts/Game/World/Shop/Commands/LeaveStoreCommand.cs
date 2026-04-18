using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Shop.Commands
{
    [Serializable]
    public sealed class LeaveStoreCommand : IGameCommand
    {
        public LeaveStoreCommand(EntityId customerId, EntityId exitPointId)
        {
            CustomerId = customerId;
            ExitPointId = exitPointId;
        }

        public EntityId CustomerId { get; }

        public EntityId ExitPointId { get; }
    }
}