namespace Game.World.Quests.Progress
{
    public interface IQuestProgressStateStore
    {
        QuestProgressState Load();
        void Save(QuestProgressState state);
    }
}