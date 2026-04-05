using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Game.World.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentifier))]
    public sealed class EntityRoot : MonoBehaviour
    {
        private List<IEntityFeature> _features = new();

        private EntityIdentifier _identifier;

        public EntityIdentifier Identifier
        {
            get
            {
                _identifier ??= GetComponent<EntityIdentifier>();
                return _identifier;
            }
        }

        public EntityId Id => new(Identifier.Id);

        public bool IsComposed => _features.Count > 0;

        public CompositeDisposable BindingDisposables { get; private set; } = new();

        public void ReplaceComposition(
            IReadOnlyList<IEntityFeature> features,
            CompositeDisposable bindingDisposables)
        {
            if (features == null)
                throw new ArgumentNullException(nameof(features));

            if (bindingDisposables == null)
                throw new ArgumentNullException(nameof(bindingDisposables));

            if (ReferenceEquals(bindingDisposables, BindingDisposables))
            {
                throw new InvalidOperationException(
                    "Cannot replace composition with the current binding disposables instance.");
            }

            var previousFeatures = _features;
            var previousBindingDisposables = BindingDisposables;

            _features = new List<IEntityFeature>(features);
            BindingDisposables = bindingDisposables;

            DisposeComposition(previousFeatures, previousBindingDisposables);
        }

        public bool TryGetFeature<TFeature>(out TFeature feature)
            where TFeature : class, IEntityFeature
        {
            foreach (var currentFeature in _features)
            {
                if (currentFeature is not TFeature typed)
                    continue;

                feature = typed;
                return true;
            }

            feature = null;
            return false;
        }

        public TFeature GetFeature<TFeature>()
            where TFeature : class, IEntityFeature
        {
            if (TryGetFeature<TFeature>(out var feature))
                return feature;

            throw new InvalidOperationException(
                $"Entity '{Id}' does not contain feature '{typeof(TFeature).Name}'.");
        }

        private void OnDestroy()
        {
            DisposeComposition(_features, BindingDisposables);

            _features = new List<IEntityFeature>();
            BindingDisposables = new CompositeDisposable();
        }

        private static void DisposeComposition(
            IReadOnlyList<IEntityFeature> features,
            CompositeDisposable bindingDisposables)
        {
            bindingDisposables.Dispose();

            foreach (var feature in features)
                feature.Dispose();
        }
    }
}