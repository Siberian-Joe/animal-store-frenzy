using System;
using Game.World.Persistence;

namespace Game.World.Store
{
    [Serializable]
    public sealed class StoreState : IEntityStateData
    {
        public StoreStatus Status = StoreStatus.Closed;

        public IEntityStateData DeepClone()
        {
            return new StoreState
            {
                Status = Status
            };
        }
    }
}
