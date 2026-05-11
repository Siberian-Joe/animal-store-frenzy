using System;
using System.Collections.Generic;

namespace Game.World.Quests.Progress
{
    [Serializable]
    public sealed class QuestProgressState
    {
        public List<QuestState> Quests = new();

        public QuestProgressState DeepClone()
        {
            var clone = new QuestProgressState();
            foreach (var quest in Quests)
            {
                if (quest == null)
                    continue;

                clone.Quests.Add(quest.DeepClone());
            }

            return clone;
        }
    }
}