using System;
using Game.World.Composition;
using Game.World.Spatial;

namespace Game.World.Navigation
{
    public sealed class NavigationModule : FeatureModule<NavigationPart, INavigationFeature>
    {
        public override int Order => 200;

        protected override INavigationFeature CreateFeature(
            EntityCompositionContext context,
            NavigationPart part)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (part == false)
                throw new ArgumentNullException(nameof(part));

            if (context.TryGetFeature<ISpatialFeature>(out var spatialFeature) == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(NavigationModule)} requires {nameof(ISpatialFeature)} " +
                    "to be composed before navigation.");
            }

            var state = context.GetOrCreateState(() => new NavigationState
            {
                HasTarget = false,
                TargetPosition = spatialFeature.Position.Value
            });

            return new NavigationFeature(state, part.StoppingDistance);
        }
    }
}