using System;
using Game.World.Quests.Targets;
using Game.World.Upgrades;
using UnityEngine;

namespace Game.World.Quests.Conditions
{
    [CreateAssetMenu(fileName = "ShelfStageQuestCondition", menuName = "Game/World/Quests/Conditions/Shelf Stage")]
    public sealed class ShelfStageQuestConditionDefinition : QuestConditionDefinition
    {
        [SerializeField] private QuestTargetDefinition _target;
        [SerializeField] private UpgradeStageReference _requiredStage;

        public override void Validate()
        {
            if (_target == false)
                throw new InvalidOperationException(
                    $"{nameof(ShelfStageQuestConditionDefinition)} '{name}' requires target definition.");

            if (_requiredStage == null)
                throw new InvalidOperationException(
                    $"{nameof(ShelfStageQuestConditionDefinition)} '{name}' requires required stage reference.");

            _target.Validate();
            _requiredStage.Validate(name);
        }

        public override bool IsMet(IQuestTargetResolver targetResolver)
        {
            if (targetResolver == null)
                return false;

            Validate();

            if (targetResolver.TryResolve(_target.Id, out var root) == false || root == false)
                return false;

            var upgradeable = root.FindOwnedComponent<UpgradeablePart>();
            return upgradeable &&
                   upgradeable.MatchesDefinition(_requiredStage.Definition) &&
                   upgradeable.IsAtStage(_requiredStage.StageId);
        }
    }
}