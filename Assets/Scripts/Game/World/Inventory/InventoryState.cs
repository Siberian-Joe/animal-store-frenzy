using System;
using System.Collections.Generic;
using Game.World.Persistence;

namespace Game.World.Inventory
{
    [Serializable]
    public sealed class InventoryState : IEntityStateData
    {
        public List<InventoryItemState> Items = new();

        public IEntityStateData DeepClone()
        {
            var clone = new InventoryState();
            foreach (var item in Items)
            {
                if (item == null)
                    continue;

                clone.Items.Add(new InventoryItemState
                {
                    ItemId = item.ItemId,
                    Amount = item.Amount
                });
            }

            return clone;
        }
    }

    [Serializable]
    public sealed class InventoryItemState
    {
        public string ItemId;
        public int Amount;
    }
}