using Game.World.Inventory;
using R3;

namespace Game.World.Shop.Shelves
{
    public interface IShelfStock
    {
        ItemId AcceptedItemId { get; }
        int CurrentAmount { get; }
        int Capacity { get; }
        int AvailableCapacity { get; }
        Observable<Unit> Changed { get; }
        bool CanStock(ItemStack stack);
        void Stock(ItemStack stack);
        bool Contains(ItemId itemId, int requiredAmount);
    }
}