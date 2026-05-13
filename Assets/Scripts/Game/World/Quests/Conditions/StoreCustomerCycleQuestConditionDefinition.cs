using System;
using Game.World.Quests.Targets;
using Game.World.Store;
using UnityEngine;

namespace Game.World.Quests.Conditions
{
    [CreateAssetMenu(
        fileName = "StoreCustomerCycleQuestCondition",
        menuName = "Game/World/Quests/Conditions/Store Customer Cycle")]
    public sealed class StoreCustomerCycleQuestConditionDefinition : QuestConditionDefinition
    {
        [SerializeField] private QuestTargetDefinition _target;
        [SerializeField] private CustomerCycleStage _requiredStage = CustomerCycleStage.CustomerLeft;

        public override void Validate()
        {
            if (_target == false)
                throw new InvalidOperationException(
                    $"{nameof(StoreCustomerCycleQuestConditionDefinition)} '{name}' requires target definition.");

            if (_requiredStage <= CustomerCycleStage.None)
            {
                throw new InvalidOperationException(
                    $"{nameof(StoreCustomerCycleQuestConditionDefinition)} '{name}' requires a positive required stage.");
            }

            _target.Validate();
        }

        public override bool IsMet(IQuestTargetResolver targetResolver)
        {
            if (targetResolver == null)
                return false;

            Validate();

            if (targetResolver.TryResolve(_target.Id, out var root) == false || root == false)
                return false;

            var progress = root.FindOwnedComponent<ICustomerCycleProgress>();
            return progress != null && progress.IsAtLeast(_requiredStage);
        }
    }
}
