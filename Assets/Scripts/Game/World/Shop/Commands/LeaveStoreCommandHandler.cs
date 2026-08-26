using System;
using Game.World.Commands;
using Game.World.EntityRuntime;
using Game.World.Persistence;
using Game.World.Shop.Customers;
using Game.World.Shop.Exits;
using Game.World.Store;

namespace Game.World.Shop.Commands
{
    public sealed class LeaveStoreCommandHandler : GameCommandHandler<LeaveStoreCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly EntityActivator _entityActivator;
        private readonly IEntityFactory _entityFactory;
        private readonly IStoreRuntimeResolver _storeResolver;
        private readonly ICustomerNeedResolution _needResolution;

        public LeaveStoreCommandHandler(
            ILiveEntityRegistry liveEntityRegistry,
            EntityActivator entityActivator,
            IEntityFactory entityFactory,
            IStoreRuntimeResolver storeResolver,
            ICustomerNeedResolution needResolution)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _entityActivator = entityActivator ?? throw new ArgumentNullException(nameof(entityActivator));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
            _needResolution = needResolution ?? throw new ArgumentNullException(nameof(needResolution));
        }

        public override void Execute(LeaveStoreCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.CustomerId, out var customerRoot) == false || customerRoot == false)
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(LeaveStoreCommand)} because customer '{command.CustomerId}' is not live.");

            if (_liveEntityRegistry.TryGet(command.ExitPointId, out var exitRoot) == false || exitRoot == false)
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(LeaveStoreCommand)} because exit point '{command.ExitPointId}' is not live.");

            var exitPoint = exitRoot.FindOwnedComponent<IStoreExitPoint>();
            if (exitPoint == null)
                throw new InvalidOperationException(
                    $"Exit root '{exitRoot.Id}' has no {nameof(IStoreExitPoint)} component.");

            var basket = customerRoot.FindOwnedComponent<ICustomerBasket>();
            if (basket == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerBasket)} component.");

            var needs = customerRoot.FindOwnedComponent<ICustomerNeeds>();
            if (needs == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerNeeds)} component.");

            var checkoutProgress = customerRoot.FindOwnedComponent<ICustomerCheckoutProgress>();
            if (checkoutProgress == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerCheckoutProgress)} component.");

            if (checkoutProgress.IsWaitingForCheckout)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' cannot leave while waiting for checkout.");

            if (basket.HasItems)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' cannot leave the store while basket still contains items.");

            if (_needResolution.HasPendingShelfVisit(customerRoot, needs))
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' cannot leave while pending shelf visits remain.");
            }

            _needResolution.AbandonUnresolvableNeeds(customerRoot, needs);

            if (needs.HasActiveNeeds)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' cannot leave the store while it still has active needs.");

            var completedCheckout = checkoutProgress.IsCheckoutCompleted;

            if (completedCheckout)
                MarkCustomerCycleStage(CustomerCycleStage.CustomerLeft);

            _entityActivator.RemoveState(customerRoot);
            _entityActivator.Deactivate(customerRoot);
            _entityFactory.Destroy(customerRoot);
        }

        private void MarkCustomerCycleStage(CustomerCycleStage stage)
        {
            if (_storeResolver.TryGetAnyCycleProgressWriter(out var progress) == false)
                return;

            progress.Mark(stage);

            if (stage == CustomerCycleStage.CustomerLeft &&
                _storeResolver.TryGetAnyStatus(out var storeStatus) &&
                storeStatus.IsOpen == false)
            {
                progress.Mark(CustomerCycleStage.StoreClosed);
            }
        }
    }
}