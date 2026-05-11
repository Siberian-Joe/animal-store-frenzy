using Game.World.Quests.Targets;
using UnityEngine;

namespace Game.World.Quests.Conditions
{
    public abstract class QuestConditionDefinition : ScriptableObject
    {
        public virtual void Validate()
        {
        }

        public abstract bool IsMet(IQuestTargetResolver targetResolver);
    }
}