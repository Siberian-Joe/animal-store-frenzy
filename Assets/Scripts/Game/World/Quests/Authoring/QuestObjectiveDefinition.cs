using System;
using Game.World.Quests.Conditions;
using UnityEngine;

namespace Game.World.Quests.Authoring
{
    [Serializable]
    public sealed class QuestObjectiveDefinition
    {
        [SerializeField] private string _objectiveId;
        [SerializeField] private string _objectiveText;
        [SerializeField] private QuestConditionDefinition _condition;

        public QuestObjectiveId Id => new(_objectiveId);
        public string ObjectiveText => string.IsNullOrWhiteSpace(_objectiveText) ? _objectiveId : _objectiveText;
        public QuestConditionDefinition Condition => _condition;

        public void Validate(string questName)
        {
            if (string.IsNullOrWhiteSpace(_objectiveId))
                throw new InvalidOperationException($"Quest '{questName}' contains an objective with empty id.");

            if (_condition == false)
                throw new InvalidOperationException(
                    $"Quest '{questName}' objective '{_objectiveId}' requires condition.");

            _condition.Validate();
        }
    }
}