using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Pickups
{
    public sealed class PickupInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Pickup Interaction")] [SerializeField]
        private string _interactionActionId = "pickup-item";

        [SerializeField] private PickupPart _pickup;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _pickup ??= GetComponent<PickupPart>();
            if (_pickup == false)
                return;

            if (actor.TryGetRole<IInventoryRole>(out var inventoryRole) == false)
                return;

            var stack = _pickup.Contents;
            if (inventoryRole.CanAccept(stack) == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            var targetRoot = GetComponentInParent<EntityRoot>();

            if (actorRoot == false || targetRoot == false)
                return;

            var command = new PickupItemCommand(
                actorRoot.Id,
                targetRoot.Id,
                stack.ItemId.Value,
                stack.Amount);

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                targetRoot,
                stack.ItemId.Value,
                stack.Amount));
        }
    }
}