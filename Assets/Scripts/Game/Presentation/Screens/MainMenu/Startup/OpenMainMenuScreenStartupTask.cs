using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Contracts.Navigation;
using Modules.Startup.Runtime.Contracts;

namespace Game.Presentation.Screens.MainMenu.Startup
{
    [Startup(StartupPhase.Activation, Order = 0)]
    public sealed class OpenMainMenuScreenStartupTask : ISceneStartupTask
    {
        private readonly IPanelNavigator _panelNavigator;

        public string Name => "Open main menu screen";

        public OpenMainMenuScreenStartupTask(IPanelNavigator panelNavigator) => _panelNavigator =
            panelNavigator ?? throw new ArgumentNullException(nameof(panelNavigator));

        public UniTask ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            _panelNavigator.Open<MainMenuScreenPresenter>();

            return UniTask.CompletedTask;
        }
    }
}