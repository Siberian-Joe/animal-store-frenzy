using System;
using Game.World.Core;
using R3;
using UnityEngine;

namespace Game.World.ProductContainer
{
    public sealed class ProductContainerFeature : EntityFeature, IProductContainerFeature
    {
        private readonly ProductContainerState _state;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<int> Quantity { get; }

        public int Capacity => _state.Capacity;
        public int FreeSpace => Mathf.Max(0, Capacity - Quantity.Value);
        public bool IsEmpty => Quantity.Value <= 0;
        public bool IsFull => Quantity.Value >= Capacity;

        public ProductContainerFeature(ProductContainerState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));

            _state.Capacity = Mathf.Max(0, _state.Capacity);
            _state.Quantity = Mathf.Clamp(_state.Quantity, 0, _state.Capacity);

            Quantity = new ReactiveProperty<int>(_state.Quantity);

            Quantity
                .Subscribe(value => _state.Quantity = Mathf.Clamp(value, 0, _state.Capacity))
                .AddTo(_disposables);
        }

        public int AddUpTo(int amount)
        {
            if (amount <= 0 || IsFull)
                return 0;

            var accepted = Mathf.Min(amount, FreeSpace);
            Quantity.Value += accepted;
            return accepted;
        }

        public int RemoveUpTo(int amount)
        {
            if (amount <= 0 || IsEmpty)
                return 0;

            var removed = Mathf.Min(amount, Quantity.Value);
            Quantity.Value -= removed;
            return removed;
        }

        public override void Dispose()
        {
            _disposables.Dispose();
            Quantity.Dispose();

            base.Dispose();
        }
    }
}
