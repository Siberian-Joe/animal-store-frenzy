using System;
using Game.World.Inventory;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerBasketItemState
    {
        public string ItemId;
        public int Quantity;

        public CustomerBasketItemState DeepClone()
        {
            return new CustomerBasketItemState
            {
                ItemId = ItemId,
                Quantity = Quantity
            };
        }

        public bool Matches(ItemId itemId) =>
            string.Equals(ItemId, itemId.Value, StringComparison.Ordinal);
    }
}