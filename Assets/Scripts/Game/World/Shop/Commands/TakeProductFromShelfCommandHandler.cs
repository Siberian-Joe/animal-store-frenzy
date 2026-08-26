using System;
using Game.World.Commands;
using Game.World.Inventory;
using Game.World.Persistence;
using Game.World.Shop.Customers;
using Game.World.Shop.Shelves;
using Game.World.Store;

namespace Game.World.Shop.Commands
{
    public sealed class TakeProductFromShelfCommandHandler : GameCommandHandler<TakeProductFromShelfCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly IStoreRuntimeResolver _storeResolver;

        public TakeProductFromShelfCommandHandler(
            ILiveEntityRegistry liveEntityRegistry,
            IStoreRuntimeResolver storeResolver)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

        public override void Execute(TakeProductFromShelfCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.CustomerId, out var customerRoot) == false || customerRoot == false)
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(TakeProductFromShelfCommand)} because customer '{command.CustomerId}' is not live.");

            if (_liveEntityRegistry.TryGet(command.ShelfId, out var shelfRoot) == false || shelfRoot == false)
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(TakeProductFromShelfCommand)} because shelf '{command.ShelfId}' is not live.");

            var customerNeeds = customerRoot.FindOwnedComponent<ICustomerNeeds>();
            if (customerNeeds == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerNeeds)} component.");

            var customerBasket = customerRoot.FindOwnedComponent<ICustomerBasket>();
            if (customerBasket == null)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerBasket)} component.");

            var shelf = shelfRoot.FindOwnedComponent<IShelfItemSource>();
            if (shelf == null)
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' has no {nameof(IShelfItemSource)} component.");

            var itemId = new ItemId(command.ItemId);

            if (customerNeeds.WantsItem(itemId) == false)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' does not currently need item '{itemId}'.");

            if (command.Quantity != 1)
                throw new InvalidOperationException(
                    $"{nameof(TakeProductFromShelfCommand)} currently supports only single-item takes. " +
                    $"Received quantity: {command.Quantity}.");

            if (shelf.ItemId != itemId)
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' stores item '{shelf.ItemId}' instead of '{itemId}'.");

            if (shelf.CurrentQuantity < command.Quantity)
            {
                if (TryAbandonItemNeed(customerNeeds, itemId) == false)
                {
                    throw new InvalidOperationException(
                        $"Customer '{customerRoot.Id}' failed to abandon unavailable need for '{itemId}' at empty shelf '{shelfRoot.Id}'.");
                }

                return;
            }

            if (shelf.TryTake(command.Quantity) == false)
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' failed to provide '{itemId}' x{command.Quantity}.");

            if (customerNeeds.TrySatisfyItemNeed(itemId) == false)
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' failed to satisfy need for '{itemId}' after shelf take.");

            customerBasket.AddItem(itemId, command.Quantity);
            MarkCustomerCycleStage(CustomerCycleStage.ProductTaken);
        }

        private static bool TryAbandonItemNeed(ICustomerNeeds customerNeeds, ItemId itemId)
        {
            var needs = customerNeeds.Needs;
            if (needs == null || needs.Count == 0)
                return false;

            for (var index = 0; index < needs.Count; index++)
            {
                var need = needs[index];
                if (need == null || need.Intensity <= 0f)
                    continue;

                if (string.Equals(need.ItemId, itemId.Value, StringComparison.Ordinal) == false)
                    continue;

                return customerNeeds.TryAbandonNeed(need.NeedId);
            }

            return false;
        }

        private void MarkCustomerCycleStage(CustomerCycleStage stage)
        {
            if (_storeResolver.TryGetAnyCycleProgressWriter(out var progress))
                progress.Mark(stage);
        }
    }
}
