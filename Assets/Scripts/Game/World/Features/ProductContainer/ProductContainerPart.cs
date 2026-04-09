using Game.World.EntityRuntime;
using Game.World.Interactions;
using R3;
using UnityEngine;

namespace Game.World.Features.ProductContainer
{
    [DisallowMultipleComponent]
    public sealed class ProductContainerPart : StatefulEntityComponent<ProductContainerState>,
        IProductContainerFeature,
        IRestockTargetContract,
        ITakeSourceContract
    {
        [SerializeField] [Min(0)] private int _capacity = 10;
        [SerializeField] [Min(0)] private int _initialQuantity;

        private readonly ReactiveProperty<int> _quantity = new(0);

        public override int ActivationOrder => 300;

        public ReactiveProperty<int> Quantity => _quantity;

        public int Capacity => HasBoundState ? State.Capacity : Mathf.Max(0, _capacity);

        public int FreeSpace => Mathf.Max(0, Capacity - _quantity.Value);

        public bool IsEmpty => _quantity.Value <= 0;

        public bool IsFull => _quantity.Value >= Capacity;

        public int AvailableQuantity => _quantity.Value;

        public int InitialQuantity => Mathf.Clamp(_initialQuantity, 0, Mathf.Max(0, _capacity));

        protected override void RestoreState(ProductContainerState state)
        {
            state.Capacity = Mathf.Max(0, state.Capacity);
            state.Quantity = Mathf.Clamp(state.Quantity, 0, state.Capacity);
        }

        protected override void InitializeFreshState(ProductContainerState state)
        {
            state.Capacity = Mathf.Max(0, _capacity);
            state.Quantity = InitialQuantity;
        }

        protected override void OnStateReady(ProductContainerState state)
        {
            _quantity.Value = state.Quantity;
        }

        protected override void OnActivate()
        {
            _quantity
                .Subscribe(value => State.Quantity = Mathf.Clamp(value, 0, State.Capacity))
                .AddTo(ActivationDisposables);
        }

        public int AddUpTo(int amount)
        {
            if (amount <= 0 || IsFull)
                return 0;

            var accepted = Mathf.Min(amount, FreeSpace);
            _quantity.Value += accepted;
            return accepted;
        }

        public int RemoveUpTo(int amount)
        {
            if (amount <= 0 || IsEmpty)
                return 0;

            var removed = Mathf.Min(amount, _quantity.Value);
            _quantity.Value -= removed;
            return removed;
        }

        public void ReturnUpTo(int amount)
        {
            if (amount <= 0)
                return;

            AddUpTo(amount);
        }

        protected override void OnDestroy()
        {
            _quantity.Dispose();
            base.OnDestroy();
        }
    }
}