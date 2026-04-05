using System;
using System.Collections.Generic;
using System.Linq;
using Game.World.Core;

namespace Game.World.Composition
{
    public sealed class EntityComposer : IEntityComposer
    {
        private readonly IEntityStateStore _stateStore;
        private readonly IReadOnlyList<ICompositionModule> _modules;

        public EntityComposer(
            IEntityStateStore stateStore,
            List<ICompositionModule> modules)
        {
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            _modules = modules?
                           .OrderBy(module => module.Order)
                           .ToArray()
                       ?? throw new ArgumentNullException(nameof(modules));
        }

        public void Compose(EntityRoot root)
        {
            if (root == false)
                throw new ArgumentNullException(nameof(root));

            var persistedState = _stateStore.GetOrCreate(
                root.Id,
                () => new EntityState(root.Id));

            var workingState = persistedState.Clone();

            using var context = new EntityCompositionContext(root, workingState);

            foreach (var module in _modules)
                module.Compose(context);

            var features = context.CreateFeatureSnapshot();

            root.ReplaceComposition(features, context.BindingDisposables);
            context.MarkCommitted();

            _stateStore.Save(workingState);
        }
    }
}