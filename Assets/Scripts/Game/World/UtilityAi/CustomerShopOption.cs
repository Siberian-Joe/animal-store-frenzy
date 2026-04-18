using System;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public abstract class CustomerShopOption : IInteractionBackedUtilityOption
    {
        public EntityRoot TargetRoot { get; }

        public InteractionTargetPart InteractionTarget { get; }

        public Vector3 ApproachPoint { get; }

        public float NormalizedDistance { get; }

        protected CustomerShopOption(
            EntityRoot targetRoot,
            InteractionTargetPart interactionTarget,
            Vector3 approachPoint,
            float normalizedDistance)
        {
            TargetRoot = targetRoot ? targetRoot : throw new ArgumentNullException(nameof(targetRoot));
            InteractionTarget = interactionTarget;
            ApproachPoint = approachPoint;
            NormalizedDistance = Mathf.Clamp01(normalizedDistance);
        }

        public abstract void WriteFacts(IUtilityFactWriter writer);

        public abstract bool IsEquivalentTo(IUtilityOption other);

        public abstract bool MatchesInteraction(InteractionOption option);
    }
}