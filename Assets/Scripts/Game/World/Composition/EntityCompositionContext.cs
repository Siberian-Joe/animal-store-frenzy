using System;
using System.Collections.Generic;
using System.Linq;
using Game.World.Core;
using R3;
using UnityEngine;
using EntityId = Game.World.Core.EntityId;

namespace Game.World.Composition
{
    public sealed class EntityCompositionContext : IDisposable
    {
        private readonly Dictionary<Type, IEntityFeature> _features = new();
        private readonly Dictionary<Type, PartCacheEntry> _partCache = new();

        private bool _isCommitted;

        public EntityId Id { get; }
        public EntityState State { get; }
        public EntityRoot Root { get; }
        public GameObject GameObject => Root.gameObject;
        public Transform Transform => Root.transform;

        public CompositeDisposable BindingDisposables { get; } = new();

        public EntityCompositionContext(EntityRoot root, EntityState state)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Id = state.Id;
        }

        public TState GetOrCreateState<TState>(Func<TState> factory)
            where TState : class, IEntityStateData =>
            State.GetOrCreate(factory);

        public bool TryGetState<TState>(out TState state)
            where TState : class, IEntityStateData =>
            State.TryGet(out state);

        public void AddFeature<TFeature>(TFeature feature)
            where TFeature : class, IEntityFeature
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));

            var contractType = typeof(TFeature);

            if (_features.TryGetValue(contractType, out var existing))
            {
                throw new InvalidOperationException(
                    $"Entity '{Id}' already contains feature contract '{contractType.Name}'. " +
                    $"Existing feature: '{existing.GetType().Name}', " +
                    $"new feature: '{feature.GetType().Name}'.");
            }

            _features.Add(contractType, feature);
        }

        public bool TryGetFeature<TFeature>(out TFeature feature)
            where TFeature : class, IEntityFeature
        {
            foreach (var entry in _features.Values)
            {
                if (entry is not TFeature typed)
                    continue;

                feature = typed;
                return true;
            }

            feature = null;
            return false;
        }

        public TFeature GetRequiredFeature<TFeature>()
            where TFeature : class, IEntityFeature
        {
            if (TryGetFeature<TFeature>(out var feature))
                return feature;

            throw new InvalidOperationException(
                $"Entity '{Id}' does not contain required feature '{typeof(TFeature).Name}'.");
        }

        public IReadOnlyList<IEntityFeature> CreateFeatureSnapshot() =>
            _features.Values.ToArray();

        public void MarkCommitted() => _isCommitted = true;

        public IReadOnlyList<TPart> GetParts<TPart>()
            where TPart : class, IFeaturePart
        {
            if (_partCache.TryGetValue(typeof(TPart), out var cached))
            {
                if (cached is PartCacheEntry<TPart> typedEntry)
                    return typedEntry.Parts;

                throw new InvalidOperationException(
                    $"Part cache entry for '{typeof(TPart).Name}' has invalid runtime type.");
            }

            var parts = GameObject
                .GetComponentsInChildren<TPart>(true)
                .Where(BelongsToCurrentRoot)
                .ToArray();

            _partCache.Add(typeof(TPart), new PartCacheEntry<TPart>(parts));
            return parts;
        }

        public void Dispose()
        {
            if (_isCommitted)
                return;

            BindingDisposables.Dispose();

            foreach (var feature in _features.Values)
                feature.Dispose();

            _features.Clear();
        }

        private bool BelongsToCurrentRoot<TPart>(TPart part)
            where TPart : class, IFeaturePart
        {
            if (part is not Component component)
            {
                throw new InvalidOperationException(
                    $"Feature part '{typeof(TPart).Name}' must be a Unity Component.");
            }

            var ownerRoot = component.GetComponentInParent<EntityRoot>();
            return ownerRoot == Root;
        }

        private abstract class PartCacheEntry
        {
        }

        private sealed class PartCacheEntry<TPart> : PartCacheEntry
            where TPart : class, IFeaturePart
        {
            public IReadOnlyList<TPart> Parts { get; }

            public PartCacheEntry(IReadOnlyList<TPart> parts)
            {
                Parts = parts ?? throw new ArgumentNullException(nameof(parts));
            }
        }
    }
}