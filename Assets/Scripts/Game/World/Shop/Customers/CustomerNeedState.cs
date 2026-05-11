using System;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerNeedState
    {
        public string NeedId;
        public string ItemId;
        public float Intensity;

        public CustomerNeedState DeepClone()
        {
            return new CustomerNeedState
            {
                NeedId = NeedId,
                ItemId = ItemId,
                Intensity = Intensity
            };
        }
    }
}