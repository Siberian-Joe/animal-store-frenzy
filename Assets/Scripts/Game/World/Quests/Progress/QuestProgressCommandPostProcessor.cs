using System;
using Game.World.Commands;

namespace Game.World.Quests.Progress
{
    public sealed class QuestProgressCommandPostProcessor : IGameCommandPostProcessor
    {
        private readonly IQuestProgressService _questProgressService;

        public QuestProgressCommandPostProcessor(IQuestProgressService questProgressService) => _questProgressService =
            questProgressService ?? throw new ArgumentNullException(nameof(questProgressService));

        public void Process(IGameCommand command) => _questProgressService.CheckProgress();
    }
}