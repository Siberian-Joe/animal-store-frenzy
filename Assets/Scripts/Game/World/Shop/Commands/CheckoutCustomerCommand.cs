using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Shop.Commands
{
    [Serializable]
    public sealed class CheckoutCustomerCommand : IGameCommand
    {
        public CheckoutCustomerCommand(
            EntityId customerId,
            EntityId checkoutId,
            int itemCount)
        {
            if (itemCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemCount), itemCount,
                    "Checkout item count must be positive.");

            CustomerId = customerId;
            CheckoutId = checkoutId;
            ItemCount = itemCount;
        }

        public EntityId CustomerId { get; }

        public EntityId CheckoutId { get; }

        public int ItemCount { get; }
    }
}