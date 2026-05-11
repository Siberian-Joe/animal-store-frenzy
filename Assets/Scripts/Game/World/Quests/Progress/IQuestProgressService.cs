namespace Game.World.Quests.Progress
{
    public interface IQuestProgressService : IQuestProgressReader
    {
        void Initialize();
        void CheckProgress(bool emitCompletion = true);
    }
}