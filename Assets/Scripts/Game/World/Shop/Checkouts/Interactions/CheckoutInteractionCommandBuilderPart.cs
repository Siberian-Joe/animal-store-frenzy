using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Persistence;
using Game.World.Shop.Commands;
using Game.World.Shop.Customers;
using UnityEngine;
using Zenject;
using EntityId = Game.World.EntityRuntime.EntityId;

namespace Game.World.Shop.Checkouts.Interactions
{
    public sealed class CheckoutInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Checkout Interaction")] [SerializeField]
        private string _interactionActionId = "checkout-customer";

        private ILiveEntityRegistry _liveEntities;
        private ICustomerNeedResolution _needResolution;

        [Inject]
        public void Construct(
            ILiveEntityRegistry liveEntities,
            ICustomerNeedResolution needResolution)
        {
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _needResolution = needResolution ?? throw new ArgumentNullException(nameof(needResolution));
        }

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (actor.TryGetRole<ICustomerBasketRole>(out var customerBasketRole))
            {
                CollectCustomerApproachOption(actor, customerBasketRole, approachPoint, options);
                return;
            }

            CollectPlayerCheckoutOption(approachPoint, options);
        }

        private void CollectCustomerApproachOption(
            IInteractionActor actor,
            ICustomerBasketRole basketRole,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (basketRole.HasItems == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var customerRoot = actorComponent.GetComponentInParent<EntityRoot>();
            if (customerRoot == false)
                return;

            var checkoutProgress = customerRoot.FindOwnedComponent<ICustomerCheckoutProgress>();
            if (checkoutProgress == null ||
                checkoutProgress.IsCheckoutCompleted ||
                checkoutProgress.IsWaitingForCheckout)
            {
                return;
            }

            var customerNeeds = customerRoot.FindOwnedComponent<ICustomerNeeds>();
            if (customerNeeds == null)
                return;

            if (_needResolution == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CheckoutInteractionCommandBuilderPart)} requires {nameof(ICustomerNeedResolution)} injection.");
            }

            if (_needResolution.HasPendingShelfVisit(customerRoot, customerNeeds))
                return;

            var checkoutRoot = GetComponentInParent<EntityRoot>();
            if (checkoutRoot == false)
                return;

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                new WaitForCheckoutCommand(customerRoot.Id, checkoutRoot.Id),
                checkoutRoot,
                subjectId: null,
                quantity: basketRole.TotalItemCount));
        }

        private void CollectPlayerCheckoutOption(
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (_liveEntities == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CheckoutInteractionCommandBuilderPart)} requires {nameof(ILiveEntityRegistry)} injection.");
            }

            var checkoutRoot = GetComponentInParent<EntityRoot>();
            if (checkoutRoot == false)
                return;

            if (TryFindWaitingCustomer(checkoutRoot.Id, out var customerRoot, out var basket) == false)
                return;

            AddCheckoutOption(
                customerRoot,
                checkoutRoot,
                basket.TotalItemCount,
                approachPoint,
                options);
        }

        private bool TryFindWaitingCustomer(
            EntityId checkoutId,
            out EntityRoot customerRoot,
            out ICustomerBasket basket)
        {
            customerRoot = null;
            basket = null;

            foreach (var candidate in _liveEntities.All)
            {
                if (candidate == false)
                    continue;

                var progress = candidate.FindOwnedComponent<ICustomerCheckoutProgress>();
                if (progress == null || progress.IsWaitingAt(checkoutId) == false)
                    continue;

                var candidateBasket = candidate.FindOwnedComponent<ICustomerBasket>();
                if (candidateBasket == null || candidateBasket.HasItems == false)
                    continue;

                if (customerRoot != false)
                {
                    throw new InvalidOperationException(
                        $"Checkout '{checkoutId}' has multiple waiting customers. " +
                        "The current store slice supports one active customer and has no checkout queue.");
                }

                customerRoot = candidate;
                basket = candidateBasket;
            }

            return customerRoot != false;
        }

        private void AddCheckoutOption(
            EntityRoot customerRoot,
            EntityRoot checkoutRoot,
            int itemCount,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (itemCount <= 0)
                return;

            var command = new CheckoutCustomerCommand(
                customerRoot.Id,
                checkoutRoot.Id,
                itemCount);

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                checkoutRoot,
                subjectId: null,
                quantity: itemCount));
        }
    }
}