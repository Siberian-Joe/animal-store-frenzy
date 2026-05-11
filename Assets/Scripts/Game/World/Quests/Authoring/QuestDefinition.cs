using System;
using System.Collections.Generic;
using Game.World.Authoring;
using UnityEngine;

namespace Game.World.Quests.Authoring
{
    [CreateAssetMenu(fileName = "QuestDefinition", menuName = "Game/World/Quests/Quest Definition")]
    public sealed class QuestDefinition : StableIdDefinition
    {
        [SerializeField] private string _title;
        [SerializeField] private string _summary;
        [SerializeField] private QuestObjectiveDefinition[] _objectives;

        public QuestId Id => new(StableIdValue);
        public string Title => string.IsNullOrWhiteSpace(_title) ? StableIdValue : _title;
        public string Summary => _summary ?? string.Empty;

        public IReadOnlyList<QuestObjectiveDefinition> Objectives =>
            _objectives ?? Array.Empty<QuestObjectiveDefinition>();

        protected override void OnValidate()
        {
            base.OnValidate();
            _title = _title?.Trim();
        }

        public void Validate()
        {
            ValidateStableId();

            if (_objectives == null || _objectives.Length == 0)
                throw new InvalidOperationException($"Quest '{name}' requires at least one objective.");

            var objectiveIds = new HashSet<QuestObjectiveId>();
            foreach (var objective in _objectives)
            {
                if (objective == null)
                    throw new InvalidOperationException($"Quest '{name}' contains null objective.");

                objective.Validate(name);
                if (objectiveIds.Add(objective.Id) == false)
                    throw new InvalidOperationException(
                        $"Quest '{name}' contains duplicate objective '{objective.Id}'.");
            }
        }
    }
}