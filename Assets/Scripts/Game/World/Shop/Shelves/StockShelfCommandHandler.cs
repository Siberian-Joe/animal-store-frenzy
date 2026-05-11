using System;
using Game.World.Commands;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using Game.World.Persistence;
using Game.World.PlayerInteraction;
using Game.World.Upgrades;

namespace Game.World.Shop.Shelves
{
    public sealed class StockShelfCommandHandler : GameCommandHandler<StockShelfCommand>
    {
        private readonly ILiveEntityRegistry _liveEntities;
        private readonly IPlayerFeedback _feedback;

        public StockShelfCommandHandler(ILiveEntityRegistry liveEntities, IPlayerFeedback feedback)
        {
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
        }

        public override void Execute(StockShelfCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntities.TryGet(command.ActorId, out var actorRoot) == false || actorRoot == false)
                throw new InvalidOperationException(
                    $"Cannot stock shelf because actor '{command.ActorId}' is not live.");

            if (_liveEntities.TryGet(command.ShelfId, out var shelfRoot) == false || shelfRoot == false)
                throw new InvalidOperationException(
                    $"Cannot stock shelf because shelf '{command.ShelfId}' is not live.");

            var inventory = actorRoot.FindOwnedComponent<IInventory>();
            if (inventory == null)
                throw new InvalidOperationException($"Actor '{actorRoot.Id}' has no {nameof(IInventory)} component.");

            ValidateRequiredStage(command, shelfRoot);

            var shelfStock = shelfRoot.FindOwnedComponent<IShelfStock>();
            if (shelfStock == null)
                throw new InvalidOperationException($"Shelf '{shelfRoot.Id}' has no {nameof(IShelfStock)} component.");

            var stack = new ItemStack(new ItemId(command.ItemId), command.Amount);
            if (shelfStock.CanStock(stack) == false)
                throw new InvalidOperationException($"Shelf '{shelfRoot.Id}' cannot accept '{stack}'.");

            if (inventory.TryRemove(stack) == false)
                throw new InvalidOperationException($"Actor '{actorRoot.Id}' does not have '{stack}' to stock shelf.");

            shelfStock.Stock(stack);
            _feedback.ShowMessage($"Stocked: {stack.Amount} {stack.ItemId}");
        }

        private static void ValidateRequiredStage(StockShelfCommand command, EntityRoot shelfRoot)
        {
            if (string.IsNullOrWhiteSpace(command.RequiredStageId))
                return;

            var upgradeable = shelfRoot.FindOwnedComponent<UpgradeablePart>();
            if (upgradeable == false)
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' requires stage '{command.RequiredStageId}' but has no {nameof(UpgradeablePart)} component.");

            var requiredStage = new UpgradeStageId(command.RequiredStageId);
            if (upgradeable.IsAtStage(requiredStage) == false)
                throw new InvalidOperationException(
                    $"Shelf '{shelfRoot.Id}' must be at stage '{requiredStage}' before stocking.");
        }
    }
}