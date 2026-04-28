using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class EntityState
    {
        private readonly Dictionary<StateSlotKey, StateEntry> _states = new();
        private string _blueprintId = string.Empty;

        public EntityId Id { get; }

        public string BlueprintId
        {
            get => _blueprintId;
            set => _blueprintId = string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
        }

        public EntityState(EntityId id, string blueprintId = "")
        {
            Id = id;
            BlueprintId = blueprintId;
        }

        public void Add(StateSlotKey key, IEntityStateData state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            ValidateStateType(key, state.GetType());

            if (_states.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    $"State entry for '{key}' is already registered for entity '{Id}'.");
            }

            _states.Add(key, new StateEntry<IEntityStateData>(state));
        }

        public bool TryGet<TState>(StateSlotKey key, out TState state)
            where TState : class, IEntityStateData
        {
            ValidateStateType(key, typeof(TState));

            if (_states.TryGetValue(key, out var existing) &&
                existing.Value is TState typedState)
            {
                state = typedState;
                return true;
            }

            state = null;
            return false;
        }

        public bool TryGet(StateSlotKey key, Type stateType, out IEntityStateData state)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            ValidateStateType(key, stateType);

            if (_states.TryGetValue(key, out var existing))
            {
                if (stateType.IsInstanceOfType(existing.Value) == false)
                {
                    throw new InvalidOperationException(
                        $"State entry '{key}' for entity '{Id}' contains '{existing.Value.GetType().Name}' instead of '{stateType.Name}'.");
                }

                state = existing.Value;
                return true;
            }

            state = null;
            return false;
        }

        public EntityState Clone()
        {
            var clone = new EntityState(Id, BlueprintId);

            foreach (var pair in _states)
                clone._states.Add(pair.Key, pair.Value.Clone());

            return clone;
        }

        public IEnumerable<StateSlice> EnumerateSlices()
        {
            foreach (var pair in _states)
                yield return new StateSlice(pair.Key, pair.Value.Value);
        }

        private static void ValidateStateType(StateSlotKey key, Type stateType)
        {
            var expectedStateTypeId = StateSlotKey.GetStateTypeId(stateType);
            if (string.Equals(key.StateTypeId, expectedStateTypeId, StringComparison.Ordinal))
                return;

            throw new InvalidOperationException(
                $"State slot key '{key}' does not match state type '{stateType.Name}'.");
        }

        private abstract class StateEntry
        {
            public abstract IEntityStateData Value { get; }

            public abstract StateEntry Clone();
        }

        public readonly struct StateSlice
        {
            public StateSlotKey Key { get; }

            public IEntityStateData State { get; }

            public StateSlice(StateSlotKey key, IEntityStateData state)
            {
                Key = key;
                State = state ?? throw new ArgumentNullException(nameof(state));
            }
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