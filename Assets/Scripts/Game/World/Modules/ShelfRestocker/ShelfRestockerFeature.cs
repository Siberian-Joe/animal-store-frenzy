using Game.World.Core;

namespace Game.World.ShelfRestocker
{
    public sealed class ShelfRestockerFeature : EntityFeature, IShelfRestockerFeature
    {
        public int TransferAmount { get; }

        public ShelfRestockerFeature(int transferAmount) => TransferAmount = transferAmount > 0 ? transferAmount : 1;
    }
}
