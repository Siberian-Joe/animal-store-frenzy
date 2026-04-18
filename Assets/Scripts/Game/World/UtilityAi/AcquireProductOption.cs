using System;
using Game.World.Features.InteractionTarget;
using Game.World.Interactions;
using Game.World.Shop.Shelves;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public sealed class AcquireProductOption : CustomerShopOption
    {
        public CustomerNeedHandle Need { get; }
        public ShelfProductPart Shelf { get; }

        public AcquireProductOption(
            CustomerNeedHandle need,
            ShelfProductPart shelf,
            InteractionTargetPart interactionTarget,
            Vector3 approachPoint,
            float normalizedDistance)
            : base(shelf.OwnerRoot, interactionTarget, approachPoint, normalizedDistance)
        {
            Need = need;
            Shelf = shelf ? shelf : throw new ArgumentNullException(nameof(shelf));
        }

        public override void WriteFacts(IUtilityFactWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            writer.Set(new CustomerNeedFact(Need));
            writer.Set(new NeedIntensityFact(Need.Intensity));
            writer.Set(new DistanceToTargetFact(NormalizedDistance));
        }

        public override bool IsEquivalentTo(IUtilityOption other) =>
            other is AcquireProductOption typed &&
            typed.TargetRoot == TargetRoot &&
            typed.Need.Equals(Need);

        public override bool MatchesInteraction(InteractionOption option) =>
            option != null &&
            option.TargetRoot == TargetRoot &&
            string.Equals(option.SubjectId, Need.ProductId.Value, StringComparison.Ordinal);
    }
}