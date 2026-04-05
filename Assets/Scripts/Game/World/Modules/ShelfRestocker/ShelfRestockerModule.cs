using System;
using Game.World.Composition;

namespace Game.World.ShelfRestocker
{
    public sealed class ShelfRestockerModule : FeatureModule<ShelfRestockerPart, IShelfRestockerFeature>
    {
        public override int Order => 350;

        protected override IShelfRestockerFeature CreateFeature(
            EntityCompositionContext context,
            ShelfRestockerPart part)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (part == false)
                throw new ArgumentNullException(nameof(part));

            return new ShelfRestockerFeature(part.TransferAmount);
        }
    }
}
