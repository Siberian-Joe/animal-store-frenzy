using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Contracts.Preparation;
using Modules.Startup.Contracts;

namespace Game.Presentation.Screens.MainMenu.Startup
{
    [Startup(StartupPhase.Preparation, Order = 0)]
    public sealed class PrepareMainMenuPresentationStartupTask : IStartupTask
    {
        public string Name => "Prepare main menu panels";

        private readonly IPanelPreparer _preloader;

        public PrepareMainMenuPresentationStartupTask(IPanelPreparer preloader)
        {
            _preloader = preloader ?? throw new ArgumentNullException(nameof(preloader));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            await _preloader.PrepareAsync<MainMenuScreenPresenter>(token);
        }
    }
}