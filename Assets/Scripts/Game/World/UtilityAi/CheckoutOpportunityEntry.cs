using System;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Shop.Checkouts;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct CheckoutOpportunityEntry
    {
        public CheckoutCounterPart Checkout { get; }
        public EntityRoot Root { get; }
        public InteractionTargetPart InteractionTarget { get; }

        public bool HasInteractionTarget => InteractionTarget != false;

        public Vector3 ApproachPoint => HasInteractionTarget
            ? InteractionTarget.ApproachPoint
            : Root.transform.position;

        public CheckoutOpportunityEntry(
            CheckoutCounterPart checkout,
            EntityRoot root,
            InteractionTargetPart interactionTarget)
        {
            Checkout = checkout ? checkout : throw new ArgumentNullException(nameof(checkout));
            Root = root ? root : throw new ArgumentNullException(nameof(root));
            InteractionTarget = interactionTarget;
        }
    }
}