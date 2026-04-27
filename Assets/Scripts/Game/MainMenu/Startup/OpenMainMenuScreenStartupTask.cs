using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.MainMenu.Presentation;
using Game.Presentation.Contracts.Navigation;
using Game.Startup.Contracts;

namespace Game.MainMenu.Startup
{
    [Startup(StartupPhase.Activation, Order = 0)]
    public sealed class OpenMainMenuScreenStartupTask : IStartupTask
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