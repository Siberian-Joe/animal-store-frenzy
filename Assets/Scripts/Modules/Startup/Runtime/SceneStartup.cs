using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Readiness.Runtime.Contracts;
using Modules.Startup.Runtime.Contracts;

namespace Modules.Startup.Runtime
{
    public sealed class SceneStartup : PipelineStartup<ISceneStartupTask>
    {
        private readonly ISceneReadyGate _sceneReadyGate;

        public SceneStartup(
            StartupPipeline<ISceneStartupTask> pipeline,
            ISceneReadyGate sceneReadyGate)
            : base(pipeline) =>
            _sceneReadyGate = sceneReadyGate ?? throw new ArgumentNullException(nameof(sceneReadyGate));

        protected override async UniTask OnSucceededAsync(CancellationToken token)
        {
            await UniTask.SwitchToMainThread(token);
            _sceneReadyGate.Open();
        }
    }
}