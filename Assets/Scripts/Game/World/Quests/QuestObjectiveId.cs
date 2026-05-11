using System;

namespace Game.World.Quests
{
    [Serializable]
    public readonly struct QuestObjectiveId : IEquatable<QuestObjectiveId>
    {
        public string Value { get; }

        public QuestObjectiveId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("QuestObjectiveId cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(QuestObjectiveId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is QuestObjectiveId other && Equals(other);
        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        public override string ToString() => Value;
        public static bool operator ==(QuestObjectiveId left, QuestObjectiveId right) => left.Equals(right);
        public static bool operator !=(QuestObjectiveId left, QuestObjectiveId right) => left.Equals(right) == false;
    }
}