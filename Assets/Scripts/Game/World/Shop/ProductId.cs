using System;

namespace Game.World.Shop
{
    [Serializable]
    public readonly struct ProductId : IEquatable<ProductId>
    {
        public string Value { get; }

        public ProductId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ProductId cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(ProductId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object obj) =>
            obj is ProductId other && Equals(other);

        public override int GetHashCode() =>
            Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public override string ToString() => Value;

        public static bool operator ==(ProductId left, ProductId right) => left.Equals(right);
        public static bool operator !=(ProductId left, ProductId right) => left.Equals(right) == false;

        public static explicit operator ProductId(string value) => new(value);
        public static implicit operator string(ProductId value) => value.Value;
    }
}