using UniRx;
using UnityEngine;

namespace Core.Enums
{
    public class ShelfModel : InteractableEntityModel
    {
        public ReactiveProperty<int> CurrentCapacity { get; }
        public int MaxCapacity { get; }

        public ShelfModel(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            CurrentCapacity = new ReactiveProperty<int>(0);
        }

        public void AddItem(int amount)
        {
            CurrentCapacity.Value = Mathf.Min(MaxCapacity, CurrentCapacity.Value + amount);
        }

        public void RemoveItem(int amount)
        {
            CurrentCapacity.Value = Mathf.Max(0, CurrentCapacity.Value - amount);
        }
    }
}