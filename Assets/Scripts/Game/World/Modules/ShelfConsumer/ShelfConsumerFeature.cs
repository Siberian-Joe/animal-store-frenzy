using Game.World.Core;

namespace Game.World.ShelfConsumer
{
    public sealed class ShelfConsumerFeature : EntityFeature, IShelfConsumerFeature
    {
        public int TransferAmount { get; }

        public ShelfConsumerFeature(int transferAmount) => TransferAmount = transferAmount > 0 ? transferAmount : 1;
    }
}
