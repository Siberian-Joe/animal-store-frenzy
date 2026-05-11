using Game.World.Authoring;
using UnityEngine;

namespace Game.World.Quests.Targets
{
    [CreateAssetMenu(fileName = "QuestTargetDefinition", menuName = "Game/World/Quests/Quest Target Definition")]
    public sealed class QuestTargetDefinition : StableIdDefinition
    {
        public QuestTargetId Id => new(StableIdValue);

        public void Validate() => ValidateStableId();
    }
}