using System;
using System.Collections.Generic;

namespace Game.World.Quests.Progress
{
    [Serializable]
    public sealed class QuestState
    {
        public string QuestId;
        public bool IsCompleted;
        public List<QuestObjectiveState> Objectives = new();

        public QuestState DeepClone()
        {
            var clone = new QuestState
            {
                QuestId = QuestId,
                IsCompleted = IsCompleted
            };

            foreach (var objective in Objectives)
            {
                if (objective == null)
                    continue;

                clone.Objectives.Add(new QuestObjectiveState
                {
                    ObjectiveId = objective.ObjectiveId,
                    IsCompleted = objective.IsCompleted
                });
            }

            return clone;
        }
    }
}