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
            string productId,
            int quantity)
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentException("Product id cannot be null or whitespace.", nameof(productId));

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Take quantity must be positive.");

            CustomerId = customerId;
            ShelfId = shelfId;
            ProductId = productId.Trim();
            Quantity = quantity;
        }

        public EntityId CustomerId { get; }

        public EntityId ShelfId { get; }

        public string ProductId { get; }

        public int Quantity { get; }
    }
}