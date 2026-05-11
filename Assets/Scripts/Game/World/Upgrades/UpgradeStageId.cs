using System;

namespace Game.World.Upgrades
{
    [Serializable]
    public readonly struct UpgradeStageId : IEquatable<UpgradeStageId>
    {
        public string Value { get; }

        public UpgradeStageId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("UpgradeStageId cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(UpgradeStageId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is UpgradeStageId other && Equals(other);
        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        public override string ToString() => Value;

        public static bool operator ==(UpgradeStageId left, UpgradeStageId right) => left.Equals(right);
        public static bool operator !=(UpgradeStageId left, UpgradeStageId right) => left.Equals(right) == false;

        public static explicit operator UpgradeStageId(string value) => new(value);
        public static implicit operator string(UpgradeStageId value) => value.Value;
    }
}