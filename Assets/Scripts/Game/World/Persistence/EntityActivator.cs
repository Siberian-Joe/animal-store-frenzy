using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class EntityActivator
    {
        private readonly IEntityStateStore _stateStore;
        private readonly ILiveEntityRegistry _liveEntities;
        private readonly IEntityComponentDiscovery _componentDiscovery;
        private readonly IEntityStateBinder _stateBinder;

        private readonly Dictionary<EntityRoot, IReadOnlyList<IEntityComponent>> _activeRoots = new();

        public EntityActivator(
            IEntityStateStore stateStore,
            ILiveEntityRegistry liveEntities,
            IEntityComponentDiscovery componentDiscovery,
            IEntityStateBinder stateBinder)
        {
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _componentDiscovery = componentDiscovery ?? throw new ArgumentNullException(nameof(componentDiscovery));
            _stateBinder = stateBinder ?? throw new ArgumentNullException(nameof(stateBinder));
        }

        public void Activate(EntityRoot root, string blueprintId = "")
        {
            if (root == false)
                throw new ArgumentNullException(nameof(root));

            if (_activeRoots.ContainsKey(root))
            {
                throw new InvalidOperationException(
                    $"Entity '{root.Id}' is already active. Hot reactivation is not supported.");
            }

            var components = _componentDiscovery.Discover(root);
            var activatedComponents = new List<IEntityComponent>(components.Count);

            try
            {
                _liveEntities.Register(root);

                var persistedState = _stateStore.TryGet(root.Id, out var existingState)
                    ? existingState
                    : new EntityState(root.Id, blueprintId);

                var workingState = persistedState.Clone();
                if (string.IsNullOrWhiteSpace(blueprintId) == false)
                    workingState.BlueprintId = blueprintId;

                _stateBinder.Bind(components, workingState);

                foreach (var component in components)
                {
                    component.Activate();
                    activatedComponents.Add(component);
                }

                _stateStore.Save(workingState);
                _activeRoots.Add(root, components);
            }
            catch
            {
                Rollback(activatedComponents);
                _liveEntities.Unregister(root);
                throw;
            }
        }

        public void RemoveState(EntityRoot root)
        {
            if (root == false)
                return;

            _stateStore.Remove(root.Id);
        }

        public void Deactivate(EntityRoot root)
        {
            if (root == false)
                return;

            if (_activeRoots.Remove(root, out var components) == false)
                return;

            try
            {
                for (var index = components.Count - 1; index >= 0; index--)
                    components[index].Deactivate();
            }
            finally
            {
                _liveEntities.Unregister(root);
            }
        }

        private static void Rollback(IReadOnlyList<IEntityComponent> activatedComponents)
        {
            for (var index = activatedComponents.Count - 1; index >= 0; index--)
                activatedComponents[index].Deactivate();
        }
    }
}