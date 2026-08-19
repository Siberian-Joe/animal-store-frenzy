using Game.World.EntityRuntime;

namespace Game.World.Shop.Customers
{
    public interface ICustomerCheckoutProgressWriter : ICustomerCheckoutProgress
    {
        void MarkWaitingForCheckout(EntityId checkoutId);
        void MarkCheckoutCompleted();
        void ResetCheckoutProgress();
    }
}