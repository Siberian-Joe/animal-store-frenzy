using Game.World.EntityRuntime;

namespace Game.World.Quests.Targets
{
    public interface IQuestTargetRegistry
    {
        void Register(QuestTargetId id, EntityRoot root);
        void Unregister(QuestTargetId id, EntityRoot root);
    }
}