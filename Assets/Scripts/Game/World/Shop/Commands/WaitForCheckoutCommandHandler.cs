using System;
using Game.World.Commands;
using Game.World.Persistence;
using Game.World.Shop.Checkouts;
using Game.World.Shop.Customers;

namespace Game.World.Shop.Commands
{
    public sealed class WaitForCheckoutCommandHandler : GameCommandHandler<WaitForCheckoutCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly ICustomerNeedResolution _needResolution;

        public WaitForCheckoutCommandHandler(
            ILiveEntityRegistry liveEntityRegistry,
            ICustomerNeedResolution needResolution)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _needResolution = needResolution ?? throw new ArgumentNullException(nameof(needResolution));
        }

        public override void Execute(WaitForCheckoutCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.CustomerId, out var customerRoot) == false || customerRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(WaitForCheckoutCommand)} because customer '{command.CustomerId}' is not live.");
            }

            if (_liveEntityRegistry.TryGet(command.CheckoutId, out var checkoutRoot) == false || checkoutRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(WaitForCheckoutCommand)} because checkout '{command.CheckoutId}' is not live.");
            }

            if (checkoutRoot.FindOwnedComponent<ICheckoutServicePoint>() == null)
            {
                throw new InvalidOperationException(
                    $"Checkout root '{checkoutRoot.Id}' has no {nameof(ICheckoutServicePoint)} component.");
            }

            var needs = customerRoot.FindOwnedComponent<ICustomerNeeds>();
            if (needs == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerNeeds)} component.");

            var basket = customerRoot.FindOwnedComponent<ICustomerBasket>();
            if (basket == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerBasket)} component.");

            var progress = customerRoot.FindOwnedComponent<ICustomerCheckoutProgressWriter>();
            if (progress == null)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerCheckoutProgressWriter)} component.");
            }

            if (progress.IsCheckoutCompleted)
                throw new InvalidOperationException($"Customer '{customerRoot.Id}' has already completed checkout.");

            if (progress.IsWaitingAt(command.CheckoutId))
                return;

            if (progress.IsWaitingForCheckout)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' is already waiting at another checkout.");
            }

            if (basket.HasItems == false)
                throw new InvalidOperationException($"Customer '{customerRoot.Id}' has no items to checkout.");

            if (_needResolution.HasPendingShelfVisit(customerRoot, needs))
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' cannot wait for checkout while pending shelf visits remain.");
            }

            _needResolution.AbandonUnresolvableNeeds(customerRoot, needs);

            if (needs.HasActiveNeeds)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' still has unresolved needs after unresolvable needs were abandoned.");
            }

            progress.MarkWaitingForCheckout(command.CheckoutId);
        }
    }
}