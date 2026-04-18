namespace Game.World.Shop.Shelves
{
    public interface IShelfProductSource
    {
        ProductId ProductId { get; }
        int CurrentQuantity { get; }
        bool HasStock { get; }

        bool TryTake(int quantity);
        void Restock(int quantity);
    }
}