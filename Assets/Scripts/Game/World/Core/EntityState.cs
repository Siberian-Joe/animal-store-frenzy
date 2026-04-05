using System;
using System.Collections.Generic;

namespace Game.World.Core
{
    public sealed class EntityState
    {
        private readonly Dictionary<Type, StateEntry> _states = new();

        public EntityId Id { get; }

        public EntityState(EntityId id) => Id = id;

        public TState GetOrCreate<TState>(Func<TState> factory)
            where TState : class, IEntityStateData
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (_states.TryGetValue(typeof(TState), out var existing))
            {
                if (existing is StateEntry<TState> typedEntry)
                    return typedEntry.Value;

                throw new InvalidOperationException(
                    $"State entry for '{typeof(TState).Name}' has invalid runtime type.");
            }

            var created = factory();
            if (created == null)
            {
                throw new InvalidOperationException(
                    $"Factory for state '{typeof(TState).Name}' returned null.");
            }

            _states.Add(typeof(TState), new StateEntry<TState>(created));
            return created;
        }

        public bool TryGet<TState>(out TState state)
            where TState : class, IEntityStateData
        {
            if (_states.TryGetValue(typeof(TState), out var existing) &&
                existing is StateEntry<TState> typedEntry)
            {
                state = typedEntry.Value;
                return true;
            }

            state = null;
            return false;
        }

        public EntityState Clone()
        {
            var clone = new EntityState(Id);

            foreach (var pair in _states)
                clone._states.Add(pair.Key, pair.Value.Clone());

            return clone;
        }

        private abstract class StateEntry
        {
            public abstract StateEntry Clone();
        }

        private sealed class StateEntry<TState> : StateEntry
            where TState : class, IEntityStateData
        {
            public TState Value { get; }

            public StateEntry(TState value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public override StateEntry Clone()
            {
                if (Value.DeepClone() is not TState cloned)
                {
                    throw new InvalidOperationException(
                        $"DeepClone for '{typeof(TState).Name}' returned invalid result.");
                }

                return new StateEntry<TState>(cloned);
            }
        }
    }
}