using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Shop.Commands;
using Game.World.Shop.Customers;
using UnityEngine;

namespace Game.World.Shop.Shelves.Interactions
{
    public sealed class ShelfTakeInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Shelf Take Interaction")] [SerializeField]
        private string _interactionActionId = "take-product-from-shelf";

        [SerializeField] private ShelfStockPart _shelfStock;

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
            if (_shelfStock == false)
                return;

            if (_shelfStock.HasStock == false)
                return;

            if (actor.TryGetRole<ICustomerShoppingRole>(out var shoppingRole) == false)
                return;

            if (shoppingRole.WantsItem(_shelfStock.ItemId) == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            if (actorRoot == false)
                return;

            var targetRoot = GetComponentInParent<EntityRoot>();
            if (targetRoot == false)
                return;

            const int quantity = 1;

            if (_shelfStock.CurrentQuantity < quantity)
                return;

            var command = new TakeProductFromShelfCommand(
                actorRoot.Id,
                targetRoot.Id,
                _shelfStock.ItemId.Value,
                quantity);

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                targetRoot,
                _shelfStock.ItemId.Value,
                quantity));
        }
    }
}