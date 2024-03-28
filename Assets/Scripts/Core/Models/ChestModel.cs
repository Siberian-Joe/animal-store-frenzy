using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Core.Models
{
    public class ChestModel : InteractableEntityModel
    {
        public ReactiveProperty<bool> Opened { get; } = new(false);
        public List<string> Items { get; } = new();

        private static readonly List<string> PossibleItems = new()
        {
            "Меч", "Щит", "Зелье здоровья", "Лук", "Зелье маны", "Кольцо силы"
        };

        public ChestModel()
        {
            GenerateRandomItems();
        }

        private void GenerateRandomItems()
        {
            var itemsCount = Random.Range(1, PossibleItems.Count);
            for (var i = 0; i < itemsCount; i++)
            {
                var itemIndex = Random.Range(0, PossibleItems.Count);
                var item = PossibleItems[itemIndex];
                if (!Items.Contains(item))
                {
                    Items.Add(item);
                }
            }
        }
    }
}