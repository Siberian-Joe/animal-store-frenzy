using R3;

namespace Game.World.Quests.Progress
{
    public interface IQuestProgressReader
    {
        QuestProgressSnapshot Current { get; }
        Observable<QuestProgressSnapshot> ProgressChanged { get; }
        Observable<QuestSnapshot> QuestCompleted { get; }
    }
}