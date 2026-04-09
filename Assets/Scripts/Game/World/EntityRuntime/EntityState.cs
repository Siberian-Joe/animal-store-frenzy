using System;
using System.Collections.Generic;

namespace Game.World.EntityRuntime
{
    public sealed class EntityState
    {
        private readonly Dictionary<Type, StateEntry> _states = new();

        public EntityId Id { get; }

        public EntityState(EntityId id) => Id = id;

        public void Add(IEntityStateData state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var stateType = state.GetType();
            if (_states.ContainsKey(stateType))
            {
                throw new InvalidOperationException(
                    $"State entry for '{stateType.Name}' is already registered for entity '{Id}'.");
            }

            _states.Add(stateType, new StateEntry<IEntityStateData>(state));
        }

        public bool TryGet<TState>(out TState state)
            where TState : class, IEntityStateData
        {
            if (_states.TryGetValue(typeof(TState), out var existing) &&
                existing.Value is TState typedState)
            {
                state = typedState;
                return true;
            }

            state = null;
            return false;
        }

        public bool TryGet(Type stateType, out IEntityStateData state)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            if (_states.TryGetValue(stateType, out var existing))
            {
                state = existing.Value;
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
            public abstract IEntityStateData Value { get; }

            public abstract StateEntry Clone();
        }

        private sealed class StateEntry<TState> : StateEntry
            where TState : class, IEntityStateData
        {
            private readonly TState _value;

            public override IEntityStateData Value => _value;

            public StateEntry(TState value)
            {
                _value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public override StateEntry Clone()
            {
                if (_value.DeepClone() is not TState cloned)
                {
                    throw new InvalidOperationException(
                        $"DeepClone for '{typeof(TState).Name}' returned invalid result.");
                }

                return new StateEntry<TState>(cloned);
            }
        }
    }
}
