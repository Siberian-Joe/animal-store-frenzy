using Game.World.Core;

namespace Game.World.ShelfConsumer
{
    public interface IShelfConsumerFeature : IEntityFeature
    {
        int TransferAmount { get; }
    }
}
