using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Shop.Commands;
using Game.World.Shop.Customers;
using UnityEngine;

namespace Game.World.Shop.Checkouts.Interactions
{
    public sealed class CheckoutInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Checkout Interaction")] [SerializeField]
        private string _interactionActionId = "checkout-customer";

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (actor.TryGetRole<ICustomerBasketRole>(out var basketRole) == false)
                return;

            if (basketRole.HasItems == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            if (actorRoot == false)
                return;

            var checkoutRoot = GetComponentInParent<EntityRoot>();
            if (checkoutRoot == false)
                return;

            var command = new CheckoutCustomerCommand(
                actorRoot.Id,
                checkoutRoot.Id,
                basketRole.TotalItemCount);

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                checkoutRoot,
                subjectId: null,
                quantity: basketRole.TotalItemCount));
        }
    }
}