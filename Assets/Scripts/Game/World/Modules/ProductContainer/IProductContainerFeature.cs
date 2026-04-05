using R3;

using Game.World.Core;

namespace Game.World.ProductContainer
{
    public interface IProductContainerFeature : IEntityFeature
    {
        ReactiveProperty<int> Quantity { get; }

        int Capacity { get; }
        int FreeSpace { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }

        int AddUpTo(int amount);
        int RemoveUpTo(int amount);
    }
}
