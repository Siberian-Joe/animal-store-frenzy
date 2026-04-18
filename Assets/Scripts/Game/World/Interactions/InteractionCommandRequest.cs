using System;
using Game.World.Commands;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Interactions
{
    [Serializable]
    public readonly struct InteractionActionId : IEquatable<InteractionActionId>
    {
        public string Value { get; }

        public InteractionActionId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Interaction action id cannot be null or whitespace.", nameof(value));

            Value = value.Trim();
        }

        public bool Equals(InteractionActionId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is InteractionActionId other && Equals(other);

        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public override string ToString() => Value;

        public static bool operator ==(InteractionActionId left, InteractionActionId right) => left.Equals(right);

        public static bool operator !=(InteractionActionId left, InteractionActionId right) => left.Equals(right) == false;
    }

    public sealed class InteractionOption
    {
        public InteractionOption(
            InteractionActionId actionId,
            Vector3 approachPoint,
            IGameCommand command,
            EntityRoot targetRoot,
            string subjectId = null,
            int quantity = 0)
        {
            ActionId = actionId;
            Command = command ?? throw new ArgumentNullException(nameof(command));
            TargetRoot = targetRoot;
            ApproachPoint = approachPoint;
            SubjectId = subjectId;
            Quantity = quantity;
        }

        public InteractionActionId ActionId { get; }

        public Vector3 ApproachPoint { get; }

        public IGameCommand Command { get; }

        public EntityRoot TargetRoot { get; }

        public string SubjectId { get; }

        public int Quantity { get; }
    }
}
