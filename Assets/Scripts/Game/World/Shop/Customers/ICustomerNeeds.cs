namespace Game.World.Shop.Customers
{
    public interface ICustomerNeeds
    {
        bool HasActiveNeeds { get; }

        int ActiveNeedCount { get; }

        bool WantsProduct(ProductId productId);

        bool TrySatisfyProductNeed(ProductId productId, float satisfactionAmount = 1f);
    }
}