using System;

namespace Game.World.Quests.Progress
{
    [Serializable]
    public sealed class QuestObjectiveState
    {
        public string ObjectiveId;
        public bool IsCompleted;
    }
}