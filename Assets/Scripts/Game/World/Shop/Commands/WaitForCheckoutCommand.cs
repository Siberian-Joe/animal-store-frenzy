using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Shop.Commands
{
    [Serializable]
    public sealed class WaitForCheckoutCommand : IGameCommand
    {
        public WaitForCheckoutCommand(EntityId customerId, EntityId checkoutId)
        {
            CustomerId = customerId;
            CheckoutId = checkoutId;
        }

        public EntityId CustomerId { get; }
        public EntityId CheckoutId { get; }
    }
}