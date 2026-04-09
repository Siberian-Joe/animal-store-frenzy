using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.World.EntityRuntime
{
    public sealed class EntityActivator
    {
        private readonly IEntityStateStore _stateStore;
        private readonly Dictionary<EntityRoot, ActivationRecord> _activeRoots = new();

        public EntityActivator(IEntityStateStore stateStore)
        {
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        }

        public void Activate(EntityRoot root)
        {
            if (root == false)
                throw new ArgumentNullException(nameof(root));

            if (_activeRoots.ContainsKey(root))
            {
                throw new InvalidOperationException(
                    $"Entity '{root.Id}' is already active. Hot reactivation is not supported.");
            }

            var persistedState = _stateStore.GetOrCreate(
                root.Id,
                () => new EntityState(root.Id));

            var workingState = persistedState.Clone();
            var components = DiscoverComponents(root);

            BindStates(components, workingState);

            var activatedComponents = new List<IEntityComponent>(components.Count);

            try
            {
                foreach (var component in components)
                {
                    component.Activate();
                    activatedComponents.Add(component);
                }

                _stateStore.Save(workingState);
                _activeRoots.Add(root, new ActivationRecord(components));
            }
            catch
            {
                Rollback(activatedComponents);
                throw;
            }
        }

        public void Deactivate(EntityRoot root)
        {
            if (root == false)
                return;

            if (_activeRoots.TryGetValue(root, out var record) == false)
                return;

            _activeRoots.Remove(root);

            for (var index = record.Components.Count - 1; index >= 0; index--)
                record.Components[index].Deactivate();
        }

        private static List<IEntityComponent> DiscoverComponents(EntityRoot root)
        {
            var discovered = root.GetComponentsInChildren<EntityComponent>(true);
            var components = new List<IEntityComponent>(discovered.Length);

            foreach (var component in discovered)
            {
                if (component == false)
                    continue;

                if (component.OwnerRoot != root)
                    continue;

                components.Add(component);
            }

            components.Sort(static (left, right) =>
            {
                var orderComparison = left.ActivationOrder.CompareTo(right.ActivationOrder);
                if (orderComparison != 0)
                    return orderComparison;

                var leftBehaviour = left as MonoBehaviour;
                var rightBehaviour = right as MonoBehaviour;

                return string.CompareOrdinal(
                    leftBehaviour != null ? leftBehaviour.GetType().Name : left.GetType().Name,
                    rightBehaviour != null ? rightBehaviour.GetType().Name : right.GetType().Name);
            });

            return components;
        }

        private static void BindStates(
            IReadOnlyList<IEntityComponent> components,
            EntityState workingState)
        {
            foreach (var component in components)
            {
                if (component is not IEntityStateBindingBridge bridge)
                    continue;

                if (workingState.TryGet(bridge.StateType, out var restoredState))
                {
                    bridge.BindRestoredState(restoredState);
                    continue;
                }

                var freshState = CreateFreshState(bridge.StateType);
                workingState.Add(freshState);
                bridge.BindFreshState(freshState);
            }
        }

        private static IEntityStateData CreateFreshState(Type stateType)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            if (typeof(IEntityStateData).IsAssignableFrom(stateType) == false)
            {
                throw new InvalidOperationException(
                    $"State type '{stateType.Name}' does not implement {nameof(IEntityStateData)}.");
            }

            try
            {
                if (Activator.CreateInstance(stateType) is not IEntityStateData created)
                {
                    throw new InvalidOperationException(
                        $"Infrastructure could not create state slice '{stateType.Name}'.");
                }

                return created;
            }
            catch (Exception exception) when (
                exception is MissingMethodException or MemberAccessException or TargetInvocationException)
            {
                throw new InvalidOperationException(
                    $"State slice '{stateType.Name}' must be constructible by infrastructure.",
                    exception);
            }
        }

        private static void Rollback(IReadOnlyList<IEntityComponent> activatedComponents)
        {
            for (var index = activatedComponents.Count - 1; index >= 0; index--)
                activatedComponents[index].Deactivate();
        }

        private sealed class ActivationRecord
        {
            public IReadOnlyList<IEntityComponent> Components { get; }

            public ActivationRecord(IReadOnlyList<IEntityComponent> components)
            {
                Components = components ?? throw new ArgumentNullException(nameof(components));
            }
        }
    }
}
