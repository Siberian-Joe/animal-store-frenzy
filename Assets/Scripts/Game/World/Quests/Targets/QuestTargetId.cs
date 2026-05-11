using System;

namespace Game.World.Quests.Targets
{
    [Serializable]
    public readonly struct QuestTargetId : IEquatable<QuestTargetId>
    {
        public string Value { get; }

        public QuestTargetId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("QuestTargetId cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(QuestTargetId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is QuestTargetId other && Equals(other);
        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        public override string ToString() => Value;
        public static bool operator ==(QuestTargetId left, QuestTargetId right) => left.Equals(right);
        public static bool operator !=(QuestTargetId left, QuestTargetId right) => left.Equals(right) == false;
    }
}