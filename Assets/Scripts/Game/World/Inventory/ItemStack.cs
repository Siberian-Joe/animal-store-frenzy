using System;

namespace Game.World.Inventory
{
    [Serializable]
    public readonly struct ItemStack : IEquatable<ItemStack>
    {
        public ItemStack(ItemId itemId, int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Item stack amount must be positive.");

            ItemId = itemId;
            Amount = amount;
        }

        public ItemId ItemId { get; }
        public int Amount { get; }

        public bool Equals(ItemStack other) => ItemId.Equals(other.ItemId) && Amount == other.Amount;
        public override bool Equals(object obj) => obj is ItemStack other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(ItemId, Amount);
        public override string ToString() => $"{ItemId}: {Amount}";
    }
}