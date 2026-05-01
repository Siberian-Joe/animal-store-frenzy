using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.SceneNavigation.Routes;
using Modules.SceneNavigation.Runtime.Contracts;
using Modules.Startup.Runtime.Contracts;

namespace Game.Bootstrap.Startup
{
    [Startup(StartupPhase.Activation, Order = 1000)]
    public sealed class GoToInitialSceneStartupTask : IApplicationStartupTask
    {
        private readonly ISceneNavigator _sceneNavigator;

        public string Name => "Go to initial scene";

        public GoToInitialSceneStartupTask(ISceneNavigator sceneNavigator) => _sceneNavigator =
            sceneNavigator ?? throw new ArgumentNullException(nameof(sceneNavigator));

        public UniTask ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (_sceneNavigator.TryGoTo<MainMenuRoute>() == false)
                throw new InvalidOperationException(
                    $"{nameof(ISceneNavigator)} rejected transition to {nameof(MainMenuRoute)}.");

            return UniTask.CompletedTask;
        }
    }
}