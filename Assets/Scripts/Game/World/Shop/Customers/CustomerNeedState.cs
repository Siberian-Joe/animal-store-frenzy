using System;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerNeedState
    {
        public string NeedId;
        public string ProductId;
        public float Intensity;

        public CustomerNeedState DeepClone()
        {
            return new CustomerNeedState
            {
                NeedId = NeedId,
                ProductId = ProductId,
                Intensity = Intensity
            };
        }
    }
}