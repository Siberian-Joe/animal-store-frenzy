using System;

namespace Game.World.Persistence
{
    public interface IEntityStateFactory
    {
        IEntityStateData Create(Type stateType);
    }
}