using System;
using Game.World.Persistence;

namespace Game.World.Store
{
    [Serializable]
    public sealed class StoreShiftState : IEntityStateData
    {
        public StoreShiftStatus Status = StoreShiftStatus.NotStarted;
        public int ServedCustomers;
        public int RequiredCustomers;
        public int ActiveCustomers;
        public int Revenue;

        public IEntityStateData DeepClone()
        {
            return new StoreShiftState
            {
                Status = Status,
                ServedCustomers = ServedCustomers,
                RequiredCustomers = RequiredCustomers,
                ActiveCustomers = ActiveCustomers,
                Revenue = Revenue
            };
        }
    }
}