using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Startup.Contracts;

namespace Modules.Startup.Runtime
{
    public abstract class PipelineStartup<TTask> : IStartup where TTask : IStartupTask
    {
        private readonly StartupPipeline<TTask> _pipeline;

        protected PipelineStartup(StartupPipeline<TTask> pipeline) =>
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));

        public async UniTask<StartupRunReport> RunAsync(CancellationToken token)
        {
            var report = await _pipeline.RunAsync(token);

            if (report.HasCriticalFailure)
                return report;

            await OnSucceededAsync(token);

            return report;
        }

        protected virtual UniTask OnSucceededAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return UniTask.CompletedTask;
        }
    }
}