namespace Game.World.Shop.Customers
{
    public interface ICustomerBasket
    {
        bool HasItems { get; }

        int TotalItemCount { get; }

        int UniqueItemCount { get; }

        int GetQuantity(ProductId productId);

        void AddProduct(ProductId productId, int quantity);

        void ClearBasket();
    }
}