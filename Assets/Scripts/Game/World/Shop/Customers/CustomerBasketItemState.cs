using System;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerBasketItemState
    {
        public string ProductId;
        public int Quantity;

        public CustomerBasketItemState DeepClone()
        {
            return new CustomerBasketItemState
            {
                ProductId = ProductId,
                Quantity = Quantity
            };
        }

        public bool Matches(ProductId productId) =>
            string.Equals(ProductId, productId.Value, StringComparison.Ordinal);
    }
}