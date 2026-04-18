using System;
using Game.World.Commands;
using Game.World.Persistence;
using Game.World.Shop.Customers;
using Game.World.Shop.Shelves;

namespace Game.World.Shop.Commands
{
    public sealed class TakeProductFromShelfCommandHandler : GameCommandHandler<TakeProductFromShelfCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;

        public TakeProductFromShelfCommandHandler(ILiveEntityRegistry liveEntityRegistry)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
        }

        public override void Execute(TakeProductFromShelfCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.CustomerId, out var customerRoot) == false || customerRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(TakeProductFromShelfCommand)} because customer '{command.CustomerId}' is not live.");
            }

            if (_liveEntityRegistry.TryGet(command.ShelfId, out var shelfRoot) == false || shelfRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(TakeProductFromShelfCommand)} because shelf '{command.ShelfId}' is not live.");
            }

            var customerNeeds = customerRoot.FindOwnedComponent<ICustomerNeeds>();
            if (customerNeeds == null)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerNeeds)} component.");
            }

            var customerBasket = customerRoot.FindOwnedComponent<ICustomerBasket>();
            if (customerBasket == null)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' has no {nameof(ICustomerBasket)} component.");
            }

            var shelf = shelfRoot.FindOwnedComponent<IShelfProductSource>();
            if (shelf == null)
            {
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' has no {nameof(IShelfProductSource)} component.");
            }

            var productId = new ProductId(command.ProductId);

            if (customerNeeds.WantsProduct(productId) == false)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' does not currently need product '{productId}'.");
            }

            if (command.Quantity != 1)
            {
                throw new InvalidOperationException(
                    $"{nameof(TakeProductFromShelfCommand)} currently supports only single-item takes. " +
                    $"Received quantity: {command.Quantity}.");
            }

            if (shelf.ProductId != productId)
            {
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' stores product '{shelf.ProductId}' instead of '{productId}'.");
            }

            if (shelf.CurrentQuantity < command.Quantity)
            {
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' does not have enough '{productId}'. Requested: {command.Quantity}, available: {shelf.CurrentQuantity}.");
            }

            if (shelf.TryTake(command.Quantity) == false)
            {
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' failed to provide '{productId}' x{command.Quantity}.");
            }

            if (customerNeeds.TrySatisfyProductNeed(productId) == false)
            {
                throw new InvalidOperationException(
                    $"Customer '{customerRoot.Id}' failed to satisfy need for '{productId}' after shelf take.");
            }

            customerBasket.AddProduct(productId, command.Quantity);
        }
    }
}