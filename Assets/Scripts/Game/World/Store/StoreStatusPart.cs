using Game.World.Persistence;
using UnityEngine;
using Zenject;

namespace Game.World.Store
{
    [DisallowMultipleComponent]
    public sealed class StoreStatusPart : StatefulEntityComponent<StoreState>, IStoreStatusWriter
    {
        [Inject] private readonly IStoreRuntimeRegistry _registry;

        public override int ActivationOrder => 300;

        public StoreStatus Status => State.Status;
        public bool IsOpen => Status == StoreStatus.Open;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(StoreState), "store-status");

        protected override void RestoreState(StoreState state) => Normalize(state);

        protected override void InitializeFreshState(StoreState state)
        {
            state.Status = StoreStatus.Closed;
            Normalize(state);
        }

        protected override void ApplyBoundState(StoreState state) => Normalize(state);

        protected override void OnStateActivated() => _registry?.Register(this);

        protected override void OnDeactivate()
        {
            _registry?.Unregister(this);
            base.OnDeactivate();
        }

        public void Open() => State.Status = StoreStatus.Open;

        private static void Normalize(StoreState state)
        {
            if (state.Status != StoreStatus.Open)
                state.Status = StoreStatus.Closed;
        }
    }
}
