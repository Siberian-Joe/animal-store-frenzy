using System;

namespace Game.World.Quests
{
    [Serializable]
    public readonly struct QuestId : IEquatable<QuestId>
    {
        public string Value { get; }

        public QuestId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("QuestId cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(QuestId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is QuestId other && Equals(other);
        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        public override string ToString() => Value;
        public static bool operator ==(QuestId left, QuestId right) => left.Equals(right);
        public static bool operator !=(QuestId left, QuestId right) => left.Equals(right) == false;
    }
}