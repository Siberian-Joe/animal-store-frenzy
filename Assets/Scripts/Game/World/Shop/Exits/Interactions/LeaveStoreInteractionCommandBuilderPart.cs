using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Shop.Commands;
using Game.World.Shop.Customers;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Exits.Interactions
{
    public sealed class LeaveStoreInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Leave Store Interaction")] [SerializeField]
        private string _interactionActionId = "leave-store";

        private ICustomerNeedResolution _needResolution;

        [Inject]
        public void Construct(ICustomerNeedResolution needResolution) =>
            _needResolution = needResolution ?? throw new ArgumentNullException(nameof(needResolution));

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            if (actorRoot == false)
                return;

            var customerBasket = actorRoot.FindOwnedComponent<ICustomerBasket>();
            if (customerBasket == null || customerBasket.HasItems)
                return;

            var customerNeeds = actorRoot.FindOwnedComponent<ICustomerNeeds>();
            if (customerNeeds == null)
                return;

            if (_needResolution == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(LeaveStoreInteractionCommandBuilderPart)} requires {nameof(ICustomerNeedResolution)} injection.");
            }

            if (_needResolution.HasPendingShelfVisit(actorRoot, customerNeeds))
                return;

            var checkoutProgress = actorRoot.FindOwnedComponent<ICustomerCheckoutProgress>();
            if (checkoutProgress == null || checkoutProgress.IsWaitingForCheckout)
                return;

            var exitRoot = GetComponentInParent<EntityRoot>();
            if (exitRoot == false)
                return;

            var command = new LeaveStoreCommand(actorRoot.Id, exitRoot.Id);

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                exitRoot));
        }
    }
}
