using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Startup.Runtime.Contracts;

namespace Game.World.Quests.Progress
{
    [Startup(StartupPhase.Preparation, Order = -100)]
    public sealed class InitializeQuestProgressStartupTask : ISceneStartupTask
    {
        public string Name => "Initialize quest progress";

        private readonly IQuestProgressService _questProgressService;

        public InitializeQuestProgressStartupTask(IQuestProgressService questProgressService)
        {
            _questProgressService =
                questProgressService ?? throw new ArgumentNullException(nameof(questProgressService));
        }

        public UniTask ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _questProgressService.Initialize();
            return UniTask.CompletedTask;
        }
    }
}