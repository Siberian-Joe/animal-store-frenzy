using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Contracts.Navigation;
using Modules.Presentation.Runtime.Contracts.Registry;
using Modules.SceneNavigation.Runtime.Contracts;

namespace Game.Presentation.Transitions.Loading
{
    public sealed class LoadingOverlaySceneTransitionScreen : ISceneTransitionScreen
    {
        private readonly IPanelNavigator _panelNavigator;
        private readonly IPanelRegistry _panelRegistry;

        public LoadingOverlaySceneTransitionScreen(
            IPanelNavigator panelNavigator,
            IPanelRegistry panelRegistry)
        {
            _panelNavigator = panelNavigator ?? throw new ArgumentNullException(nameof(panelNavigator));
            _panelRegistry = panelRegistry ?? throw new ArgumentNullException(nameof(panelRegistry));
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            var handle = _panelRegistry.Get<LoadingOverlayPresenter>();

            if (handle.IsOpen == false)
                _panelNavigator.Open<LoadingOverlayPresenter>();

            await handle.Presenter.FadeInAsync(token);
        }

        public async UniTask HideAsync(CancellationToken token)
        {
            var handle = _panelRegistry.Get<LoadingOverlayPresenter>();

            if (handle.IsOpen == false)
                return;

            await handle.Presenter.FadeOutAsync(token);

            _panelNavigator.Close<LoadingOverlayPresenter>();
        }
    }
}