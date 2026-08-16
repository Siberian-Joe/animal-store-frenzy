using System;
using Game.World.Inventory;
using Game.World.Quests.Targets;
using UnityEngine;

namespace Game.World.Quests.Conditions
{
    [CreateAssetMenu(
        fileName = "InventoryAmountQuestCondition",
        menuName = "Game/World/Quests/Conditions/Inventory Amount")]
    public sealed class InventoryAmountQuestConditionDefinition : QuestConditionDefinition
    {
        [SerializeField] private QuestTargetDefinition _target;
        [SerializeField] private ItemDefinition _requiredItem;
        [SerializeField, Min(1)] private int _requiredAmount = 1;

        public override void Validate()
        {
            if (_target == false)
                throw new InvalidOperationException(
                    $"{nameof(InventoryAmountQuestConditionDefinition)} '{name}' requires target definition.");

            if (_requiredItem == false)
                throw new InvalidOperationException(
                    $"{nameof(InventoryAmountQuestConditionDefinition)} '{name}' requires item definition.");

            if (_requiredAmount <= 0)
                throw new InvalidOperationException(
                    $"{nameof(InventoryAmountQuestConditionDefinition)} '{name}' requires positive required amount.");

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

            var inventory = root.FindOwnedComponent<IInventoryReader>();
            return inventory != null && inventory.GetAmount(_requiredItem.Id) >= _requiredAmount;
        }
    }
}
