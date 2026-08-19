using Game.World.EntityRuntime;

namespace Game.World.Shop.Customers
{
    public interface ICustomerCheckoutProgress
    {
        bool IsCheckoutCompleted { get; }
        bool IsWaitingForCheckout { get; }

        bool IsWaitingAt(EntityId checkoutId);
    }
}