using System.Collections.Generic;
using System.Linq;
using NewCore.Domain;
using NewCore.Modules.Interaction.Abstractions;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public class Shelf : Entity<ShelfData>, IActor
    {
        public string Name { get; }

        public ReactiveProperty<int> MaxCapacity { get; }

        public ReactiveProperty<int> Capacity { get; }

        public IReadOnlyList<IInteractionRule> Rules { get; }

        public Shelf(ShelfData model, IEnumerable<IInteractionRule> rules) : base(model)
        {
            Rules = rules.ToList();

            Name = model.Name;
            Capacity = new ReactiveProperty<int>(model.Capacity);
            MaxCapacity = new ReactiveProperty<int>(model.MaxCapacity);

            Capacity
                .Take(1)
                .Subscribe(capacity => model.Capacity = capacity)
                .AddTo(Disposables);

            MaxCapacity
                .Take(1)
                .Subscribe(level => model.MaxCapacity = level)
                .AddTo(Disposables);
        }

        public void StoreOne() => Capacity.Value = Mathf.Min(Capacity.Value + 1, MaxCapacity.Value);
        public void WithdrawOne() => Capacity.Value = Mathf.Max(Capacity.Value - 1, 0);

        protected override ShelfData CreateModel()
        {
            return new ShelfData
            {
                Id = Id,
                Name = Name,
                Position = Position.Value,
                Capacity = Capacity.Value,
                MaxCapacity = MaxCapacity.Value
            };
        }
    }
}