using System;
using Game.World.Persistence;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerCheckoutProgressState : IEntityStateData
    {
        public bool IsCheckoutCompleted;
        public string WaitingCheckoutId;

        public IEntityStateData DeepClone()
        {
            return new CustomerCheckoutProgressState
            {
                IsCheckoutCompleted = IsCheckoutCompleted,
                WaitingCheckoutId = WaitingCheckoutId
            };
        }
    }
}