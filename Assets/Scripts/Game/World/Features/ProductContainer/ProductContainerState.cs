using System;
using Game.World.EntityRuntime;

namespace Game.World.Features.ProductContainer
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