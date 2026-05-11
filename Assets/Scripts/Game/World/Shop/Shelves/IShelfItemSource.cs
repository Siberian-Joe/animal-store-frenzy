using Game.World.Inventory;

namespace Game.World.Shop.Shelves
{
    public interface IShelfItemSource
    {
        ItemId ItemId { get; }
        int CurrentQuantity { get; }
        bool HasStock { get; }

        bool TryTake(int quantity);
    }
}