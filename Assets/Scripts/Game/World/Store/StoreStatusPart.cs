using Game.World.Persistence;
using R3;
using UnityEngine;
using Zenject;

namespace Game.World.Store
{
    [DisallowMultipleComponent]
    public sealed class StoreStatusPart : StatefulEntityComponent<StoreState>, IStoreStatusWriter
    {
        [Inject] private readonly IStoreRuntimeRegistry _registry;

        private readonly Subject<Unit> _changed = new();

        public override int ActivationOrder => 300;

        public StoreStatus Status => State.Status;
        public bool IsOpen => Status == StoreStatus.Open;
        public Observable<Unit> Changed => _changed;

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

        public void Open() => SetStatus(StoreStatus.Open);
        public void Close() => SetStatus(StoreStatus.Closed);

        protected override void OnDestroy()
        {
            _changed.Dispose();
            base.OnDestroy();
        }

        private void SetStatus(StoreStatus status)
        {
            if (State.Status == status)
                return;

            State.Status = status;
            _changed.OnNext(Unit.Default);
        }

        private static void Normalize(StoreState state)
        {
            if (state.Status != StoreStatus.Open)
                state.Status = StoreStatus.Closed;
        }
    }
}
