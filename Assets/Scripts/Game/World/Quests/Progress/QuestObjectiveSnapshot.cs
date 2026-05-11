namespace Game.World.Quests.Progress
{
    public readonly struct QuestObjectiveSnapshot
    {
        public QuestObjectiveSnapshot(string objectiveId, string text, bool isCompleted)
        {
            ObjectiveId = objectiveId;
            Text = text;
            IsCompleted = isCompleted;
        }

        public string ObjectiveId { get; }
        public string Text { get; }
        public bool IsCompleted { get; }
    }
}