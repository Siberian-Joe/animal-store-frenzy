using System;
using Game.World.Composition;

namespace Game.World.Spatial
{
    public sealed class SpatialModule : FeatureModule<SpatialPart, ISpatialFeature>
    {
        public override int Order => 100;

        protected override ISpatialFeature CreateFeature(EntityCompositionContext context, SpatialPart part)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (part == false)
                throw new ArgumentNullException(nameof(part));

            var state = context.GetOrCreateState(() => new SpatialState
            {
                Position = part.transform.position,
                Rotation = part.transform.rotation
            });

            return new SpatialFeature(state);
        }
    }
}
