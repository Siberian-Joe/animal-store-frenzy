using System;
using System.Collections.Generic;

namespace Game.World.Core
{
    public sealed class InMemoryEntityStateStore : IEntityStateStore
    {
        private readonly Dictionary<EntityId, EntityState> _states = new();

        public IReadOnlyCollection<EntityState> All => _states.Values;

        public bool TryGet(EntityId id, out EntityState state) =>
            _states.TryGetValue(id, out state);

        public EntityState GetOrCreate(EntityId id, Func<EntityState> factory)
        {
            if (_states.TryGetValue(id, out var existing))
                return existing;

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var created = factory();

            if (created == null)
                throw new InvalidOperationException(
                    $"Factory for entity state '{id}' returned null.");

            _states.Add(id, created);
            return created;
        }

        public void Save(EntityState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            _states[state.Id] = state;
        }
    }
}