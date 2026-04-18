using System;
using System.Collections.Generic;
using Game.World.Persistence;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerBasketState : IEntityStateData
    {
        public List<CustomerBasketItemState> Items = new();

        public IEntityStateData DeepClone()
        {
            var clone = new CustomerBasketState();

            if (Items == null)
                return clone;

            for (var index = 0; index < Items.Count; index++)
            {
                var item = Items[index];
                if (item == null)
                    continue;

                clone.Items.Add(item.DeepClone());
            }

            return clone;
        }
    }
}