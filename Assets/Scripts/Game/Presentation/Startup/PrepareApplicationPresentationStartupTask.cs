using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Presentation.Transitions.Loading;
using Modules.Presentation.Contracts.Navigation;
using Modules.Presentation.Contracts.Preparation;
using Modules.Presentation.Contracts.Registry;
using Modules.Startup.Contracts;

namespace Game.Presentation.Startup
{
    [Startup(StartupPhase.Foundation, Order = -1090)]
    public sealed class PrepareApplicationPresentationStartupTask : IApplicationStartupTask
    {
        private readonly IPanelPreparer _preparer;
        private readonly IPanelNavigator _panelNavigator;
        private readonly IPanelRegistry _panelRegistry;

        public string Name => "Prepare application presentation";

        public PrepareApplicationPresentationStartupTask(
            IPanelPreparer preparer,
            IPanelNavigator panelNavigator,
            IPanelRegistry panelRegistry)
        {
            _preparer = preparer ?? throw new ArgumentNullException(nameof(preparer));
            _panelNavigator = panelNavigator ?? throw new ArgumentNullException(nameof(panelNavigator));
            _panelRegistry = panelRegistry ?? throw new ArgumentNullException(nameof(panelRegistry));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            await _preparer.PrepareAsync<LoadingOverlayPresenter>(token);

            var handle = _panelRegistry.Get<LoadingOverlayPresenter>();

            if (handle.IsOpen == false)
                _panelNavigator.Open<LoadingOverlayPresenter>();

            handle.Presenter.ShowImmediate();
        }
    }
}