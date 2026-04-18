using Game.World.Persistence;
using UnityEngine;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    public sealed class CustomerProfilePart :
        StatefulEntityComponent<CustomerProfileState>,
        ICustomerProfile
    {
        public override int ActivationOrder => 310;

        public string ArchetypeId => State.ArchetypeId ?? string.Empty;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(CustomerProfileState), "customer-profile");

        protected override void RestoreState(CustomerProfileState state)
        {
        }

        protected override void InitializeFreshState(CustomerProfileState state) => state.ArchetypeId = string.Empty;

        protected override void OnStateActivated() =>
            State.ArchetypeId = string.IsNullOrWhiteSpace(State.ArchetypeId)
                ? string.Empty
                : State.ArchetypeId.Trim();

        public void SetArchetype(string archetypeId) =>
            State.ArchetypeId = string.IsNullOrWhiteSpace(archetypeId)
                ? string.Empty
                : archetypeId.Trim();
    }
}