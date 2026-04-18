using Game.World.Interactions;

namespace Game.World.Shop.Customers
{
    public interface ICustomerBasketRole : IInteractionRole
    {
        bool HasItems { get; }

        int TotalItemCount { get; }
    }
}