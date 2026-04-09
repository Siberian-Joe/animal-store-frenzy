using Game.World.EntityRuntime;
using R3;

namespace Game.World.Features.ProductContainer
{
    public interface IProductContainerFeature : IEntityComponent
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
