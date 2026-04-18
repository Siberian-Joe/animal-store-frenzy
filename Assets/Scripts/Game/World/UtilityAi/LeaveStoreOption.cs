using System;
using Game.World.Features.InteractionTarget;
using Game.World.Interactions;
using Game.World.Shop.Exits;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public sealed class LeaveStoreOption : CustomerShopOption
    {
        public StoreExitPointPart ExitPoint { get; }

        public LeaveStoreOption(
            StoreExitPointPart exitPoint,
            InteractionTargetPart interactionTarget,
            Vector3 approachPoint,
            float normalizedDistance)
            : base(exitPoint.OwnerRoot, interactionTarget, approachPoint, normalizedDistance) =>
            ExitPoint = exitPoint ? exitPoint : throw new ArgumentNullException(nameof(exitPoint));

        public override void WriteFacts(IUtilityFactWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            writer.Set(new DistanceToTargetFact(NormalizedDistance));
        }

        public override bool IsEquivalentTo(IUtilityOption other) =>
            other is LeaveStoreOption typed && typed.TargetRoot == TargetRoot;

        public override bool MatchesInteraction(InteractionOption option) =>
            option != null && option.TargetRoot == TargetRoot;
    }
}