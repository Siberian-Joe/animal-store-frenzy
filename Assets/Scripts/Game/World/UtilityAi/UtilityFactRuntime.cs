using System;
using System.Collections.Generic;

namespace Game.World.UtilityAi
{
    public sealed class UtilityFactRuntime : IUtilityFactWriter
    {
        private readonly Dictionary<Type, IFactSlot> _slots = new(16);

        public IUtilityFactAccessor<TFact> GetAccessor<TFact>() where TFact : struct =>
            GetOrCreateSlot<TFact>();

        public void Set<TFact>(in TFact fact) where TFact : struct => GetOrCreateSlot<TFact>().Set(in fact);

        public void Clear()
        {
            foreach (var slot in _slots.Values)
                slot.Clear();
        }

        private FactSlot<TFact> GetOrCreateSlot<TFact>()
            where TFact : struct
        {
            if (_slots.TryGetValue(typeof(TFact), out var rawSlot))
                return (FactSlot<TFact>)rawSlot;

            var created = new FactSlot<TFact>();
            _slots.Add(typeof(TFact), created);
            return created;
        }

        private interface IFactSlot
        {
            void Clear();
        }

        private sealed class FactSlot<TFact> : IFactSlot, IUtilityFactAccessor<TFact>
            where TFact : struct
        {
            public bool HasValue { get; private set; }

            public TFact Value { get; private set; }

            public void Set(in TFact value)
            {
                Value = value;
                HasValue = true;
            }

            public void Clear()
            {
                Value = default;
                HasValue = false;
            }
        }
    }
}