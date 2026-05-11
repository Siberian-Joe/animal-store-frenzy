using System;
using Game.World.Inventory;
using Game.World.Quests.Targets;
using Game.World.Shop.Shelves;
using UnityEngine;

namespace Game.World.Quests.Conditions
{
    [CreateAssetMenu(fileName = "ShelfStockQuestCondition", menuName = "Game/World/Quests/Conditions/Shelf Stock")]
    public sealed class ShelfStockQuestConditionDefinition : QuestConditionDefinition
    {
        [SerializeField] private QuestTargetDefinition _target;
        [SerializeField] private ItemDefinition _requiredItem;
        [SerializeField, Min(1)] private int _requiredAmount = 1;

        public override void Validate()
        {
            if (_target == false)
                throw new InvalidOperationException(
                    $"{nameof(ShelfStockQuestConditionDefinition)} '{name}' requires target definition.");

            if (_requiredItem == false)
                throw new InvalidOperationException(
                    $"{nameof(ShelfStockQuestConditionDefinition)} '{name}' requires required item definition.");

            if (_requiredAmount <= 0)
                throw new InvalidOperationException(
                    $"{nameof(ShelfStockQuestConditionDefinition)} '{name}' requires positive required amount.");

            _target.Validate();
            _requiredItem.Validate();
        }

        public override bool IsMet(IQuestTargetResolver targetResolver)
        {
            if (targetResolver == null)
                return false;

            Validate();

            if (targetResolver.TryResolve(_target.Id, out var root) == false || root == false)
                return false;

            var stock = root.FindOwnedComponent<IShelfStock>();
            return stock != null && stock.Contains(_requiredItem.Id, _requiredAmount);
        }
    }
}