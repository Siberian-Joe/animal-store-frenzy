using System;
using Game.World.Composition;

namespace Game.World.ProductContainer
{
    public sealed class ProductContainerModule : FeatureModule<ProductContainerPart, IProductContainerFeature>
    {
        public override int Order => 300;

        protected override IProductContainerFeature CreateFeature(
            EntityCompositionContext context,
            ProductContainerPart part)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (part == false)
                throw new ArgumentNullException(nameof(part));

            var state = context.GetOrCreateState(() => new ProductContainerState
            {
                Capacity = part.Capacity,
                Quantity = part.InitialQuantity
            });

            return new ProductContainerFeature(state);
        }
    }
}
