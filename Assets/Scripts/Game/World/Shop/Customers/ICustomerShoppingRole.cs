using Game.World.Interactions;

namespace Game.World.Shop.Customers
{
    public interface ICustomerShoppingRole : IInteractionRole
    {
        bool WantsProduct(ProductId productId);
    }
}