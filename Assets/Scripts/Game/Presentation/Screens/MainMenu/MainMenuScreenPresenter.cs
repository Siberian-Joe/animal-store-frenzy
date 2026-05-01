using System;
using Game.SceneNavigation.Runtime.Routes;
using Modules.Presentation.Runtime.Panels;
using Modules.SceneNavigation.Runtime.Contracts;
using R3;
using UnityEngine;

namespace Game.Presentation.Screens.MainMenu
{
    public sealed class MainMenuScreenPresenter : PanelPresenter<MainMenuScreen>
    {
        private readonly ISceneNavigator _sceneNavigator;
        private readonly CompositeDisposable _disposables = new();

        public MainMenuScreenPresenter(ISceneNavigator sceneNavigator) => _sceneNavigator =
            sceneNavigator ?? throw new ArgumentNullException(nameof(sceneNavigator));

        protected override void OnPanelAttached(MainMenuScreen panel) =>
            panel.PlayRequested
                .Subscribe(_ => GoToCoreScene())
                .AddTo(_disposables);

        protected override void OnReleased() => _disposables.Dispose();

        private void GoToCoreScene()
        {
            if (_sceneNavigator.TryGoTo<CoreRoute>() == false)
                Debug.LogWarning(
                    $"{nameof(ISceneNavigator)} rejected transition to {nameof(CoreRoute)}. " +
                    "Transition is probably already running.");
        }
    }
}