using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class EntityStateBinder : IEntityStateBinder
    {
        private readonly IEntityStateFactory _stateFactory;

        public EntityStateBinder(IEntityStateFactory stateFactory)
        {
            _stateFactory = stateFactory ?? throw new ArgumentNullException(nameof(stateFactory));
        }

        public void Bind(IReadOnlyList<IEntityComponent> components, EntityState workingState)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));
            if (workingState == null)
                throw new ArgumentNullException(nameof(workingState));

            var boundSlots = new HashSet<StateSlotKey>();

            foreach (var component in components)
            {
                if (component is not IEntityStateBindingBridge bridge)
                    continue;

                var stateSlotKey = bridge.StateSlotKey;
                if (boundSlots.Add(stateSlotKey) == false)
                {
                    throw new InvalidOperationException(
                        $"Duplicate state slot '{stateSlotKey}' detected while activating entity.");
                }

                if (workingState.TryGet(stateSlotKey, bridge.StateType, out var restoredState))
                {
                    bridge.BindRestoredState(restoredState);
                    continue;
                }

                var freshState = _stateFactory.Create(bridge.StateType);
                workingState.Add(stateSlotKey, freshState);
                bridge.BindFreshState(freshState);
            }
        }
    }
}