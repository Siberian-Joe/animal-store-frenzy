using System;
using System.Linq;
using Game.World.Composition;
using Game.World.Interactions;
using Game.World.Spatial;

namespace Game.World.InteractionTarget
{
    public sealed class InteractionTargetModule : FeatureModule<InteractionTargetPart, IInteractionTargetFeature>
    {
        public override int Order => 400;

        protected override IInteractionTargetFeature CreateFeature(
            EntityCompositionContext context,
            InteractionTargetPart part)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (part == false)
                throw new ArgumentNullException(nameof(part));

            var transform = context.GetRequiredFeature<ISpatialFeature>();

            var resolvers = part.GetComponents<IInteractionResolver>()
                .OrderBy(resolver => resolver.Order)
                .ToArray();

            if (resolvers.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Interaction target on '{context.Root.name}' has no local interaction resolvers. " +
                    $"Add one or more '{nameof(InteractionResolverPart)}' components next to '{nameof(InteractionTargetPart)}'.");
            }

            return new InteractionTargetFeature(
                context.Root,
                transform,
                resolvers,
                part.LocalInteractionOffset);
        }
    }
}