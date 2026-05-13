namespace Game.World.Shop.Customers
{
    public interface ICustomerCheckoutProgressWriter : ICustomerCheckoutProgress
    {
        void MarkCheckoutCompleted();

        void ResetCheckoutProgress();
    }
}
