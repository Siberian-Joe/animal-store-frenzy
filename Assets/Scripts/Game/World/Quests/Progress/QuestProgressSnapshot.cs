using System.Collections.Generic;

namespace Game.World.Quests.Progress
{
    public readonly struct QuestProgressSnapshot
    {
        public QuestProgressSnapshot(IReadOnlyList<QuestSnapshot> quests) => Quests = quests;

        public IReadOnlyList<QuestSnapshot> Quests { get; }
    }
}