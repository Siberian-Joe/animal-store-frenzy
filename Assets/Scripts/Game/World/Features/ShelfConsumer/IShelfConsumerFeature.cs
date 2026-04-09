using Game.World.EntityRuntime;
namespace Game.World.Features.ShelfConsumer
{
    public interface IShelfConsumerFeature : IEntityComponent
    {
        int TransferAmount { get; }
    }
}
