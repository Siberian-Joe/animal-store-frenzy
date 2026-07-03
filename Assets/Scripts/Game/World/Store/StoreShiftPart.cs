using System;
using Game.World.Persistence;
using UnityEngine;
using Zenject;

namespace Game.World.Store
{
    [DisallowMultipleComponent]
    public sealed class StoreShiftPart :
        StatefulEntityComponent<StoreShiftState>,
        IStoreShiftWriter,
        IStoreShiftRewardPolicy
    {
        [Header("Store Shift")] [SerializeField, Min(1)]
        private int _requiredCustomers = 1;

        [SerializeField, Min(1)] private int _maxActiveCustomers = 1;
        [SerializeField, Min(0)] private int _fixedRewardPerCustomer = 10;
        [SerializeField] private StoreStatusPart _storeStatus;

        [Inject] private readonly IStoreRuntimeRegistry _registry;

        private FixedStoreShiftRewardPolicy _rewardPolicy;

        public override int ActivationOrder => 310;

        public StoreShiftStatus Status => State.Status;
        public int ServedCustomers => State.ServedCustomers;
        public int RequiredCustomers => State.RequiredCustomers;
        public int ActiveCustomers => State.ActiveCustomers;
        public int Revenue => State.Revenue;

        public bool CanSpawnCustomer =>
            IsStoreOpen() &&
            Status == StoreShiftStatus.Active &&
            ServedCustomers < RequiredCustomers &&
            ActiveCustomers < MaxActiveCustomers;

        public bool CanCloseStore =>
            Status == StoreShiftStatus.ReadyToClose &&
            ServedCustomers >= RequiredCustomers &&
            ActiveCustomers == 0;

        private int MaxActiveCustomers => Mathf.Max(1, _maxActiveCustomers);

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(StoreShiftState), "store-shift");

        protected override void RestoreState(StoreShiftState state) => Normalize(state);

        protected override void InitializeFreshState(StoreShiftState state)
        {
            state.Status = StoreShiftStatus.NotStarted;
            state.ServedCustomers = 0;
            state.RequiredCustomers = Mathf.Max(1, _requiredCustomers);
            state.ActiveCustomers = 0;
            state.Revenue = 0;
            Normalize(state);
        }

        protected override void ApplyBoundState(StoreShiftState state) => Normalize(state);

        protected override void OnStateActivated() => _registry?.Register(this);

        protected override void OnDeactivate()
        {
            _registry?.Unregister(this);
            base.OnDeactivate();
        }

        public void StartShift()
        {
            Normalize(State);

            if (Status == StoreShiftStatus.Completed)
                return;

            if (Status != StoreShiftStatus.NotStarted)
                return;

            State.Status = StoreShiftStatus.Active;
        }

        public void MarkCustomerEntered()
        {
            Normalize(State);

            if (Status != StoreShiftStatus.Active)
                throw new InvalidOperationException("Cannot enter customer because store shift is not active.");

            if (CanSpawnCustomer == false)
                throw new InvalidOperationException("Cannot enter customer because store shift spawn limit is reached.");

            State.ActiveCustomers++;
        }

        public void MarkCustomerServed(int reward)
        {
            Normalize(State);

            if (Status != StoreShiftStatus.Active)
                throw new InvalidOperationException("Cannot serve customer because store shift is not active.");

            if (State.ActiveCustomers <= 0)
                throw new InvalidOperationException("Cannot serve customer because no active shift customer is tracked.");

            State.ActiveCustomers--;
            State.ServedCustomers = Mathf.Min(State.RequiredCustomers, State.ServedCustomers + 1);
            State.Revenue += Mathf.Max(0, reward);

            if (State.ServedCustomers >= State.RequiredCustomers && State.ActiveCustomers == 0)
                State.Status = StoreShiftStatus.ReadyToClose;
        }

        public void CompleteShift()
        {
            Normalize(State);

            if (Status == StoreShiftStatus.Completed)
                return;

            if (CanCloseStore == false)
                throw new InvalidOperationException("Cannot complete store shift before it is ready to close.");

            State.Status = StoreShiftStatus.Completed;
        }

        public int GetReward(StoreShiftRewardContext context)
        {
            _rewardPolicy ??= new FixedStoreShiftRewardPolicy(_fixedRewardPerCustomer);
            return _rewardPolicy.GetReward(context);
        }

        private bool IsStoreOpen()
        {
            if (_storeStatus == false)
                _storeStatus = OwnerRoot ? OwnerRoot.FindOwnedComponent<StoreStatusPart>() : null;

            return _storeStatus && _storeStatus.IsOpen;
        }

        private void Normalize(StoreShiftState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            _requiredCustomers = Mathf.Max(1, _requiredCustomers);
            _maxActiveCustomers = Mathf.Max(1, _maxActiveCustomers);
            _fixedRewardPerCustomer = Mathf.Max(0, _fixedRewardPerCustomer);
            _rewardPolicy = new FixedStoreShiftRewardPolicy(_fixedRewardPerCustomer);

            if (state.Status < StoreShiftStatus.NotStarted || state.Status > StoreShiftStatus.Completed)
                state.Status = StoreShiftStatus.NotStarted;

            if (state.RequiredCustomers <= 0)
                state.RequiredCustomers = _requiredCustomers;

            state.RequiredCustomers = Mathf.Max(1, state.RequiredCustomers);
            state.ServedCustomers = Mathf.Clamp(state.ServedCustomers, 0, state.RequiredCustomers);
            state.ActiveCustomers = Mathf.Clamp(state.ActiveCustomers, 0, MaxActiveCustomers);
            state.Revenue = Mathf.Max(0, state.Revenue);

            if (state.Status == StoreShiftStatus.NotStarted)
                state.ActiveCustomers = 0;

            if (state.Status == StoreShiftStatus.Active &&
                state.ServedCustomers >= state.RequiredCustomers &&
                state.ActiveCustomers == 0)
                state.Status = StoreShiftStatus.ReadyToClose;

            if (state.Status == StoreShiftStatus.ReadyToClose &&
                (state.ServedCustomers < state.RequiredCustomers || state.ActiveCustomers > 0))
                state.Status = StoreShiftStatus.Active;

            if (state.Status == StoreShiftStatus.Completed)
                state.ActiveCustomers = 0;
        }

        private void OnValidate()
        {
            _requiredCustomers = Mathf.Max(1, _requiredCustomers);
            _maxActiveCustomers = Mathf.Max(1, _maxActiveCustomers);
            _fixedRewardPerCustomer = Mathf.Max(0, _fixedRewardPerCustomer);
        }
    }
}
