using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.MainMenu.Presentation;
using Game.Presentation.Contracts.Preparation;
using Game.Startup.Contracts;

namespace Game.MainMenu.Startup
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