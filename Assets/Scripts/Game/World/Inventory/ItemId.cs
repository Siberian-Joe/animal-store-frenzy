using System;

namespace Game.World.Inventory
{
    [Serializable]
    public readonly struct ItemId : IEquatable<ItemId>
    {
        public string Value { get; }

        public ItemId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ItemId cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(ItemId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is ItemId other && Equals(other);

        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public override string ToString() => Value;

        public static bool operator ==(ItemId left, ItemId right) => left.Equals(right);
        public static bool operator !=(ItemId left, ItemId right) => left.Equals(right) == false;

        public static explicit operator ItemId(string value) => new(value);
        public static implicit operator string(ItemId value) => value.Value;
    }
}