using System;
using Game.World.Persistence;

namespace Game.World.Shop.Shelves
{
    [Serializable]
    public sealed class ShelfInventoryState : IEntityStateData
    {
        public int CurrentQuantity;

        public IEntityStateData DeepClone()
        {
            return new ShelfInventoryState
            {
                CurrentQuantity = CurrentQuantity
            };
        }
    }
}