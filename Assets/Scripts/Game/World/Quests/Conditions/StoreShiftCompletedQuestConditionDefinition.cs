using System;
using Game.World.Quests.Targets;
using Game.World.Store;
using UnityEngine;

namespace Game.World.Quests.Conditions
{
    [CreateAssetMenu(
        fileName = "StoreShiftCompletedQuestCondition",
        menuName = "Game/World/Quests/Conditions/Store Shift Completed")]
    public sealed class StoreShiftCompletedQuestConditionDefinition : QuestConditionDefinition
    {
        [SerializeField] private QuestTargetDefinition _target;

        public override void Validate()
        {
            if (_target == false)
                throw new InvalidOperationException(
                    $"{nameof(StoreShiftCompletedQuestConditionDefinition)} '{name}' requires target definition.");

            _target.Validate();
        }

        public override bool IsMet(IQuestTargetResolver targetResolver)
        {
            if (targetResolver == null)
                return false;

            Validate();

            if (targetResolver.TryResolve(_target.Id, out var root) == false || root == false)
                return false;

            var shift = root.FindOwnedComponent<IStoreShift>();
            return shift is { Status: StoreShiftStatus.Completed };
        }
    }
}