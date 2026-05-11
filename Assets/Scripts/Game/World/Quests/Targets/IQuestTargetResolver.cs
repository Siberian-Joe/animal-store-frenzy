using Game.World.EntityRuntime;

namespace Game.World.Quests.Targets
{
    public interface IQuestTargetResolver
    {
        bool TryResolve(QuestTargetId id, out EntityRoot root);
    }
}