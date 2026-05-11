using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Shop.Commands
{
    [Serializable]
    public sealed class TakeProductFromShelfCommand : IGameCommand
    {
        public TakeProductFromShelfCommand(
            EntityId customerId,
            EntityId shelfId,
            string itemId,
            int quantity)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("Item id cannot be null or whitespace.", nameof(itemId));

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Take quantity must be positive.");

            CustomerId = customerId;
            ShelfId = shelfId;
            ItemId = itemId.Trim();
            Quantity = quantity;
        }

        public EntityId CustomerId { get; }

        public EntityId ShelfId { get; }

        public string ItemId { get; }

        public int Quantity { get; }
    }
}