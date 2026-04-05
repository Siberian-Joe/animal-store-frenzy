using System;
using Game.World.Core;

namespace Game.World.ProductContainer
{
    [Serializable]
    public sealed class ProductContainerState : IEntityStateData
    {
        public int Capacity;
        public int Quantity;

        public IEntityStateData DeepClone()
        {
            return new ProductContainerState
            {
                Capacity = Capacity,
                Quantity = Quantity
            };
        }
    }
}