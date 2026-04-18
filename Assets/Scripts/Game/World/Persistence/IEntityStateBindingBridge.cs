using System;

namespace Game.World.Persistence
{
    internal interface IEntityStateBindingBridge
    {
        StateSlotKey StateSlotKey { get; }
        Type StateType { get; }

        void BindRestoredState(IEntityStateData state);
        void BindFreshState(IEntityStateData state);
    }
}