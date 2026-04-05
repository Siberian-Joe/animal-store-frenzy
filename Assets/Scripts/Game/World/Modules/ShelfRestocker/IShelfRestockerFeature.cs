using Game.World.Core;

namespace Game.World.ShelfRestocker
{
    public interface IShelfRestockerFeature : IEntityFeature
    {
        int TransferAmount { get; }
    }
}
