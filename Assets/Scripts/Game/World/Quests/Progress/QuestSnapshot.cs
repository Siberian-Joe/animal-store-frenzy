using System.Collections.Generic;

namespace Game.World.Quests.Progress
{
    public readonly struct QuestSnapshot
    {
        public string QuestId { get; }
        public string Title { get; }
        public string Summary { get; }
        public bool IsCompleted { get; }
        public IReadOnlyList<QuestObjectiveSnapshot> Objectives { get; }

        public QuestSnapshot(
            string questId,
            string title,
            string summary,
            bool isCompleted,
            IReadOnlyList<QuestObjectiveSnapshot> objectives)
        {
            QuestId = questId;
            Title = title;
            Summary = summary;
            IsCompleted = isCompleted;
            Objectives = objectives;
        }
    }
}