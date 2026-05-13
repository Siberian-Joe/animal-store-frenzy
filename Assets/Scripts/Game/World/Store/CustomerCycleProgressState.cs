using System;
using Game.World.Persistence;

namespace Game.World.Store
{
    [Serializable]
    public sealed class CustomerCycleProgressState : IEntityStateData
    {
        public CustomerCycleStage CurrentStage = CustomerCycleStage.None;

        public IEntityStateData DeepClone()
        {
            return new CustomerCycleProgressState
            {
                CurrentStage = CurrentStage
            };
        }
    }
}
