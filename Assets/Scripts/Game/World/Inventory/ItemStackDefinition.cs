using System;
using UnityEngine;

namespace Game.World.Inventory
{
    [Serializable]
    public sealed class ItemStackDefinition
    {
        [SerializeField] private ItemDefinition _item;
        [SerializeField, Min(1)] private int _amount = 1;

        public ItemDefinition Item => _item;
        public int Amount => Mathf.Max(1, _amount);

        public ItemStack ToStack()
        {
            if (_item == false)
                throw new InvalidOperationException($"{nameof(ItemStackDefinition)} requires an item definition.");

            _item.Validate();
            return new ItemStack(_item.Id, Amount);
        }
    }
}