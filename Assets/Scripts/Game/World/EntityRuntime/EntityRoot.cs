using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.World.EntityRuntime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentifier))]
    public sealed class EntityRoot : MonoBehaviour
    {
        private EntityIdentifier _identifier;
        private OwnedComponentIndex _ownedComponentIndex;

        public EntityIdentifier Identifier
        {
            get
            {
                _identifier ??= GetComponent<EntityIdentifier>();
                return _identifier;
            }
        }

        public EntityId Id => new(Identifier.Id);

        public TComponent FindOwnedComponent<TComponent>()
            where TComponent : class
        {
            TryFindOwnedComponent(out TComponent component);
            return component;
        }

        public bool TryFindOwnedComponent<TComponent>(out TComponent component)
            where TComponent : class
        {
            EnsureOwnedComponentIndex();
            return _ownedComponentIndex.TryGet(out component);
        }

        public bool TryFindOwnedComponent<TComponent>(
            Component scope,
            out TComponent component,
            Component excluded = null)
            where TComponent : class
        {
            component = null;

            if (scope == false)
                return false;

            EnsureOwnedComponentIndex();
            return _ownedComponentIndex.TryGet(scope, out component, excluded);
        }

        public void CollectOwnedComponents<TComponent>(
            List<TComponent> results,
            Component excluded = null)
            where TComponent : class =>
            CollectOwnedComponents(this, results, excluded);

        public void CollectOwnedComponents<TComponent>(
            Component scope,
            List<TComponent> results,
            Component excluded = null)
            where TComponent : class
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            if (scope == false)
                return;

            EnsureOwnedComponentIndex();
            _ownedComponentIndex.Collect(scope, results, excluded);
        }

        public void InvalidateOwnedComponentIndex() => _ownedComponentIndex = null;

        private void OnTransformChildrenChanged() => InvalidateOwnedComponentIndex();

        private void EnsureOwnedComponentIndex() => _ownedComponentIndex ??= OwnedComponentIndex.Build(this);
    }
}