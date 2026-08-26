using Game.World.Persistence;
using UnityEngine;
using Zenject;

namespace Game.World.Store
{
    [DisallowMultipleComponent]
    public sealed class StoreCustomerCycleProgressPart :
        StatefulEntityComponent<CustomerCycleProgressState>,
        ICustomerCycleProgressWriter
    {
        [Inject] private readonly IStoreRuntimeRegistry _registry;

        public override int ActivationOrder => 305;

        public CustomerCycleStage CurrentStage => State.CurrentStage;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(CustomerCycleProgressState), "store-customer-cycle");

        protected override void RestoreState(CustomerCycleProgressState state) => Normalize(state);

        protected override void InitializeFreshState(CustomerCycleProgressState state)
        {
            state.CurrentStage = CustomerCycleStage.None;
            Normalize(state);
        }

        protected override void ApplyBoundState(CustomerCycleProgressState state) => Normalize(state);

        protected override void OnStateActivated() => _registry?.Register(this);

        protected override void OnDeactivate()
        {
            _registry?.Unregister(this);
            base.OnDeactivate();
        }

        public bool IsAtLeast(CustomerCycleStage stage) => CurrentStage >= stage;

        public void Mark(CustomerCycleStage stage)
        {
            if (stage <= CustomerCycleStage.None)
                return;

            if (stage <= State.CurrentStage)
                return;

            State.CurrentStage = stage;
        }

        private static void Normalize(CustomerCycleProgressState state)
        {
            if (state.CurrentStage < CustomerCycleStage.None)
                state.CurrentStage = CustomerCycleStage.None;

            if (state.CurrentStage > CustomerCycleStage.StoreClosed)
                state.CurrentStage = CustomerCycleStage.StoreClosed;
        }
    }
}
