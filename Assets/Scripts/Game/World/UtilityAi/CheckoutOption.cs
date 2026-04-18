using System;
using Game.World.Features.InteractionTarget;
using Game.World.Interactions;
using Game.World.Shop.Checkouts;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public sealed class CheckoutOption : CustomerShopOption
    {
        public CheckoutCounterPart Checkout { get; }

        public CheckoutOption(
            CheckoutCounterPart checkout,
            InteractionTargetPart interactionTarget,
            Vector3 approachPoint,
            float normalizedDistance)
            : base(checkout.OwnerRoot, interactionTarget, approachPoint, normalizedDistance) =>
            Checkout = checkout ? checkout : throw new ArgumentNullException(nameof(checkout));

        public override void WriteFacts(IUtilityFactWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            writer.Set(new DistanceToTargetFact(NormalizedDistance));
        }

        public override bool IsEquivalentTo(IUtilityOption other) =>
            other is CheckoutOption typed && typed.TargetRoot == TargetRoot;

        public override bool MatchesInteraction(InteractionOption option) =>
            option != null && option.TargetRoot == TargetRoot;
    }
}