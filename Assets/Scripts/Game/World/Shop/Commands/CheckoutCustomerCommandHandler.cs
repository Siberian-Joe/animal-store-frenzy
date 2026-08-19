using System;
using Game.World.Commands;
using Game.World.Persistence;
using Game.World.Shop.Checkouts;
using Game.World.Shop.Customers;
using Game.World.Store;

namespace Game.World.Shop.Commands
{
    public sealed class CheckoutCustomerCommandHandler : GameCommandHandler<CheckoutCustomerCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly IStoreRuntimeResolver _storeResolver;

        public CheckoutCustomerCommandHandler(
            ILiveEntityRegistry liveEntityRegistry,
            IStoreRuntimeResolver storeResolver)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

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

            var checkoutProgress = customerRoot.FindOwnedComponent<ICustomerCheckoutProgressWriter>();
            if (checkoutProgress == null)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerCheckoutProgressWriter)} component.");
            }

            if (checkoutProgress.IsCheckoutCompleted)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has already completed checkout.");
            }

            if (checkoutProgress.IsWaitingAt(command.CheckoutId) == false)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' is not waiting at checkout '{command.CheckoutId}'.");
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
            checkoutProgress.MarkCheckoutCompleted();
            MarkCustomerCycleStage(CustomerCycleStage.CheckoutCompleted);
        }

        private void MarkCustomerCycleStage(CustomerCycleStage stage)
        {
            if (_storeResolver.TryGetAnyCycleProgressWriter(out var progress))
                progress.Mark(stage);
        }
    }
}