using System;
using Game.World.Core;

namespace Game.World.Composition
{
    public abstract class FeatureModule<TPart, TFeature> : ICompositionModule
        where TPart : class, IFeaturePart<TFeature>
        where TFeature : class, IEntityFeature
    {
        public virtual int Order => 0;

        public void Compose(EntityCompositionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var parts = context.GetParts<TPart>();

            if (parts.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Entity '{context.Id}' contains multiple parts '{typeof(TPart).Name}' " +
                    $"for feature contract '{typeof(TFeature).Name}'. " +
                    "FeatureModule supports zero or one part per feature contract.");
            }

            if (parts.Count == 0)
                return;

            var part = parts[0];
            var feature = CreateFeature(context, part);

            if (feature == null)
            {
                throw new InvalidOperationException(
                    $"Module '{GetType().Name}' returned null for " +
                    $"part '{typeof(TPart).Name}' and feature '{typeof(TFeature).Name}'.");
            }

            context.AddFeature(feature);
            part.Bind(feature, context.BindingDisposables);

            AfterCompose(context, part, feature);
        }

        protected abstract TFeature CreateFeature(EntityCompositionContext context, TPart part);

        protected virtual void AfterCompose(
            EntityCompositionContext context,
            TPart part,
            TFeature feature)
        {
        }
    }
}