using R3;

namespace Game.World.Inventory
{
    public interface IPlayerInventoryReader
    {
        IInventoryReader Inventory { get; }
        Observable<Unit> Changed { get; }
    }
}