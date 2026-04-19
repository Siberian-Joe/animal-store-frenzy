using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.SceneReady.Contracts;
using Game.Startup.Contracts;

namespace Game.Startup.Runtime
{
    public sealed class SceneStartup : IStartup
    {
        private readonly StartupPipeline _pipeline;
        private readonly ISceneReadyGate _sceneReadyGate;

        public SceneStartup(
            StartupPipeline pipeline,
            ISceneReadyGate sceneReadyGate)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _sceneReadyGate = sceneReadyGate ?? throw new ArgumentNullException(nameof(sceneReadyGate));
        }

        public async UniTask<StartupRunReport> RunAsync(CancellationToken token)
        {
            var report = await _pipeline.RunAsync(token);

            if (report.HasCriticalFailure)
                return report;

            await UniTask.SwitchToMainThread(token);
            _sceneReadyGate.Open();

            return report;
        }
    }
}