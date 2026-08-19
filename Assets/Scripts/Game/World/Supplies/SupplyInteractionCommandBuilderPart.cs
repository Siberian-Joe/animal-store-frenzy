using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Supplies
{
    public sealed class SupplyInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Supply Interaction")] [SerializeField]
        private string _interactionActionId = "collect-supply";

        [SerializeField] private ItemSupplyPointPart _supplyPoint;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _supplyPoint ??= GetComponent<ItemSupplyPointPart>();
            if (_supplyPoint == false)
                return;

            if (actor.TryGetRole<IInventoryRole>(out var inventoryRole) == false)
                return;

            var stack = _supplyPoint.Contents;
            if (inventoryRole.CanAccept(stack) == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            var supplyRoot = GetComponentInParent<EntityRoot>();

            if (actorRoot == false || supplyRoot == false)
                return;

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                new CollectSupplyCommand(actorRoot.Id, supplyRoot.Id),
                supplyRoot,
                stack.ItemId.Value,
                stack.Amount));
        }
    }
}