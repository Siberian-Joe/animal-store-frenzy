using System;

namespace Game.World.Core
{
    [Serializable]
    public readonly struct EntityId : IEquatable<EntityId>
    {
        public string Value { get; }

        public EntityId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("EntityId cannot be null or whitespace.", nameof(value));

            Value = value;
        }

        public bool Equals(EntityId other) => Value == other.Value;

        public override bool Equals(object obj) =>
            obj is EntityId other && Equals(other);

        public override int GetHashCode() =>
            Value != null ? Value.GetHashCode() : 0;

        public override string ToString() => Value;

        public static bool operator ==(EntityId left, EntityId right) => left.Equals(right);
        public static bool operator !=(EntityId left, EntityId right) => left.Equals(right) == false;

        public static implicit operator string(EntityId id) => id.Value;
        public static explicit operator EntityId(string value) => new(value);
    }
}