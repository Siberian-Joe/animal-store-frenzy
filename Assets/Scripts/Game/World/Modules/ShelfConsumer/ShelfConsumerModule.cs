using System;
using Game.World.Composition;

namespace Game.World.ShelfConsumer
{
    public sealed class ShelfConsumerModule : FeatureModule<ShelfConsumerPart, IShelfConsumerFeature>
    {
        public override int Order => 350;

        protected override IShelfConsumerFeature CreateFeature(
            EntityCompositionContext context,
            ShelfConsumerPart part)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (part == false)
                throw new ArgumentNullException(nameof(part));

            return new ShelfConsumerFeature(part.TransferAmount);
        }
    }
}
