using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class InMemoryEntityStateStore : IEntityStateStore
    {
        private readonly Dictionary<EntityId, EntityState> _states = new();
        private readonly bool _hasSnapshot;

        public InMemoryEntityStateStore()
            : this(Array.Empty<EntityState>(), false)
        {
        }

        public InMemoryEntityStateStore(
            IEnumerable<EntityState> initialStates,
            bool hasSnapshot = true)
        {
            if (initialStates == null)
                throw new ArgumentNullException(nameof(initialStates));

            foreach (var state in initialStates)
            {
                if (state == null)
                    continue;

                _states[state.Id] = state;
            }

            _hasSnapshot = hasSnapshot;
        }

        public bool HasSnapshot => _hasSnapshot;

        public IReadOnlyCollection<EntityState> All => _states.Values;

        public bool TryGet(EntityId id, out EntityState state) =>
            _states.TryGetValue(id, out state);

        public void Save(EntityState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            _states[state.Id] = state;
        }

        public bool Remove(EntityId id) =>
            _states.Remove(id);
    }
}