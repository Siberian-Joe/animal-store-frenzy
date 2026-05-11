using System;
using Game.World.Persistence;

namespace Game.World.Shop.Shelves
{
    [Serializable]
    public sealed class ShelfStockState : IEntityStateData
    {
        public string ItemId;
        public int Amount;

        public IEntityStateData DeepClone() => new ShelfStockState
        {
            ItemId = ItemId,
            Amount = Amount
        };
    }
}