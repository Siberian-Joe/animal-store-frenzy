using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Inventory;
using Game.World.Upgrades;
using UnityEngine;

namespace Game.World.Shop.Shelves.Interactions
{
    public sealed class StockShelfInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Stock Shelf Interaction")] [SerializeField]
        private string _interactionActionId = "stock-shelf";

        [SerializeField] private UpgradeStageReference _requiredStage;
        [SerializeField] private ShelfStockPart _shelfStock;
        [SerializeField] private UpgradeablePart _upgradeable;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _shelfStock ??= GetComponent<ShelfStockPart>();
            _upgradeable ??= GetComponent<UpgradeablePart>();

            if (_shelfStock == false)
                return;

            if (IsRequiredStageSatisfied() == false)
                return;

            if (actor.TryGetRole<IInventoryRole>(out var inventoryRole) == false)
                return;

            var inventory = inventoryRole.Inventory;
            if (inventory == null)
                return;

            var itemId = _shelfStock.AcceptedItemId;
            var amount = Math.Min(inventory.GetAmount(itemId), _shelfStock.AvailableCapacity);
            if (amount <= 0)
                return;

            var stack = new ItemStack(itemId, amount);
            if (_shelfStock.CanStock(stack) == false)
                return;

            if (inventoryRole.CanPay(new[] { stack }) == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            var targetRoot = GetComponentInParent<EntityRoot>();

            if (actorRoot == false || targetRoot == false)
                return;

            var command = new StockShelfCommand(
                actorRoot.Id,
                targetRoot.Id,
                stack.ItemId.Value,
                stack.Amount,
                GetRequiredStageIdOrNull());

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                targetRoot,
                stack.ItemId.Value,
                stack.Amount));
        }

        private bool IsRequiredStageSatisfied()
        {
            if (_requiredStage == null || _requiredStage.IsEmpty)
                return true;

            _requiredStage.Validate(name);

            if (_upgradeable == false)
                return false;

            return _upgradeable.MatchesDefinition(_requiredStage.Definition) &&
                   _upgradeable.IsAtStage(_requiredStage.StageId);
        }

        private string GetRequiredStageIdOrNull()
        {
            if (_requiredStage == null || _requiredStage.IsEmpty)
                return null;

            _requiredStage.Validate(name);
            return _requiredStage.StageId.Value;
        }
    }
}