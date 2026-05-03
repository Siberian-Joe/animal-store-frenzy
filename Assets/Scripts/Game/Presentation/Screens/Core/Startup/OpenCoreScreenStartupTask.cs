using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Contracts.Navigation;
using Modules.Startup.Runtime.Contracts;

namespace Game.Presentation.Screens.Core.Startup
{
    [Startup(StartupPhase.Activation, Order = 0)]
    public sealed class OpenCoreScreenStartupTask : ISceneStartupTask
    {
        private readonly IPanelNavigator _panelNavigator;

        public string Name => "Open core screen";

        public OpenCoreScreenStartupTask(IPanelNavigator panelNavigator) => _panelNavigator =
            panelNavigator ?? throw new ArgumentNullException(nameof(panelNavigator));

        public UniTask ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            _panelNavigator.Open<CoreScreenPresenter>();

            return UniTask.CompletedTask;
        }
    }
}