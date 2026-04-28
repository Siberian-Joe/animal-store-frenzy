using Game.World.Shop.Checkouts;
using Game.World.Shop.Exits;
using Game.World.Shop.Shelves;

namespace Game.World.Shop
{
    public interface IShopInteractionRegistry
    {
        void Register(ShelfProductPart shelf);
        void Unregister(ShelfProductPart shelf);

        void Register(CheckoutCounterPart checkout);
        void Unregister(CheckoutCounterPart checkout);

        void Register(StoreExitPointPart exitPoint);
        void Unregister(StoreExitPointPart exitPoint);
    }
}