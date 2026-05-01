using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Startup.Runtime;
using Modules.Startup.Runtime.Contracts;

namespace Game.SceneComposition.Shared.Startup
{
    [Startup(StartupPhase.Foundation, Order = -10000)]
    public sealed class WaitApplicationStartupTask : ISceneStartupTask
    {
        private readonly StartupRunner<ApplicationStartup> _applicationStartupRunner;

        public string Name => "Wait application startup";

        public WaitApplicationStartupTask(
            StartupRunner<ApplicationStartup> applicationStartupRunner) =>
            _applicationStartupRunner = applicationStartupRunner
                                        ?? throw new ArgumentNullException(nameof(applicationStartupRunner));

        public async UniTask ExecuteAsync(CancellationToken token) =>
            await _applicationStartupRunner.CompletionTask
                .AttachExternalCancellation(token);
    }
}