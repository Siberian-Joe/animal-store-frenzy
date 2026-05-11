using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Pickups
{
    [Serializable]
    public sealed class PickupItemCommand : IGameCommand
    {
        public EntityId ActorId { get; }
        public EntityId PickupId { get; }
        public string ItemId { get; }
        public int Amount { get; }

        public PickupItemCommand(EntityId actorId, EntityId pickupId, string itemId, int amount)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("Item id cannot be null or whitespace.", nameof(itemId));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Pickup amount must be positive.");

            ActorId = actorId;
            PickupId = pickupId;
            ItemId = itemId.Trim();
            Amount = amount;
        }
    }
}