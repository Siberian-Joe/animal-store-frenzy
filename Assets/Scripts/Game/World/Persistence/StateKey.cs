using System;

namespace Game.World.Persistence
{
    [Serializable]
    public readonly struct StateSlotKey : IEquatable<StateSlotKey>
    {
        public string OwnerScopeId { get; }

        public string StateTypeId { get; }

        public StateSlotKey(string stateTypeId, string ownerScopeId = "")
        {
            if (string.IsNullOrWhiteSpace(stateTypeId))
                throw new ArgumentException("State type id cannot be null or whitespace.", nameof(stateTypeId));

            OwnerScopeId = string.IsNullOrWhiteSpace(ownerScopeId)
                ? string.Empty
                : ownerScopeId.Trim();
            StateTypeId = stateTypeId.Trim();
        }

        public static StateSlotKey For<TState>()
            where TState : class, IEntityStateData =>
            For(typeof(TState));

        public static StateSlotKey For<TState>(string ownerScopeId)
            where TState : class, IEntityStateData =>
            For(typeof(TState), ownerScopeId);

        public static StateSlotKey For(Type stateType)
            => For(stateType, string.Empty);

        public static StateSlotKey For(Type stateType, string ownerScopeId)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            if (typeof(IEntityStateData).IsAssignableFrom(stateType) == false)
            {
                throw new InvalidOperationException(
                    $"State type '{stateType.Name}' does not implement {nameof(IEntityStateData)}.");
            }

            return new StateSlotKey(GetStateTypeId(stateType), ownerScopeId);
        }

        public static string GetStateTypeId(Type stateType)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            return stateType.AssemblyQualifiedName ?? stateType.FullName ?? stateType.Name;
        }

        public bool Equals(StateSlotKey other) =>
            string.Equals(OwnerScopeId, other.OwnerScopeId, StringComparison.Ordinal) &&
            string.Equals(StateTypeId, other.StateTypeId, StringComparison.Ordinal);

        public override bool Equals(object obj) =>
            obj is StateSlotKey other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(
                OwnerScopeId != null ? StringComparer.Ordinal.GetHashCode(OwnerScopeId) : 0,
                StateTypeId != null ? StringComparer.Ordinal.GetHashCode(StateTypeId) : 0);

        public override string ToString() =>
            string.IsNullOrEmpty(OwnerScopeId)
                ? StateTypeId
                : $"{OwnerScopeId}::{StateTypeId}";

        public static bool operator ==(StateSlotKey left, StateSlotKey right) => left.Equals(right);
        public static bool operator !=(StateSlotKey left, StateSlotKey right) => left.Equals(right) == false;
    }
}