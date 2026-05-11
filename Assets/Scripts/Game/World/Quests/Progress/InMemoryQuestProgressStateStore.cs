namespace Game.World.Quests.Progress
{
    public sealed class InMemoryQuestProgressStateStore : IQuestProgressStateStore
    {
        private QuestProgressState _state = new();

        public QuestProgressState Load() => _state.DeepClone();

        public void Save(QuestProgressState state) => _state = state?.DeepClone() ?? new QuestProgressState();
    }
}