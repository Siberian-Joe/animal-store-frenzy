using System;
using Game.World.Commands;
using Game.World.EntityRuntime;
using Game.World.Persistence;
using Game.World.Shop.Checkouts;
using Game.World.Shop.Customers;

namespace Game.World.Shop.Commands
{
    public sealed class CheckoutCustomerCommandHandler : GameCommandHandler<CheckoutCustomerCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;

        public CheckoutCustomerCommandHandler(ILiveEntityRegistry liveEntityRegistry) => _liveEntityRegistry =
            liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));

        public override void Execute(CheckoutCustomerCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.CustomerId, out var customerRoot) == false || customerRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(CheckoutCustomerCommand)} because customer '{command.CustomerId}' is not live.");
            }

            if (_liveEntityRegistry.TryGet(command.CheckoutId, out var checkoutRoot) == false || checkoutRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(CheckoutCustomerCommand)} because checkout '{command.CheckoutId}' is not live.");
            }

            var basket = customerRoot.FindOwnedComponent<ICustomerBasket>();
            if (basket == null)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerBasket)} component.");
            }

            var checkout = checkoutRoot.FindOwnedComponent<ICheckoutServicePoint>();
            if (checkout == null)
            {
                throw new InvalidOperationException(
                    $"Checkout root '{checkoutRoot.Id}' has no {nameof(ICheckoutServicePoint)} component.");
            }

            if (basket.HasItems == false)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no items to checkout.");
            }

            if (basket.TotalItemCount != command.ItemCount)
            {
                throw new InvalidOperationException(
                    $"Checkout command item count mismatch for customer '{customerRoot.Id}'. " +
                    $"Command: {command.ItemCount}, basket: {basket.TotalItemCount}.");
            }

            basket.ClearBasket();
        }
    }
}
