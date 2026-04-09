using Game.World.EntityRuntime;

namespace Game.World.Features.ShelfRestocker
{
    public interface IShelfRestockerFeature : IEntityComponent
    {
        int TransferAmount { get; }
    }
}