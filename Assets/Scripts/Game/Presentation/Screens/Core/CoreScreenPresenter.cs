using System;
using Game.SceneNavigation.Routes;
using Modules.Presentation.Runtime.Panels;
using Modules.SceneNavigation.Runtime.Contracts;
using R3;
using UnityEngine;

namespace Game.Presentation.Screens.Core
{
    public sealed class CoreScreenPresenter : PanelPresenter<CoreScreen>
    {
        private readonly ISceneNavigator _sceneNavigator;
        private readonly CompositeDisposable _disposables = new();

        public CoreScreenPresenter(ISceneNavigator sceneNavigator) => _sceneNavigator =
            sceneNavigator ?? throw new ArgumentNullException(nameof(sceneNavigator));

        protected override void OnPanelAttached(CoreScreen panel) =>
            panel.MainMenuRequested
                .Subscribe(_ => ReturnToMainMenu())
                .AddTo(_disposables);

        protected override void OnReleased() => _disposables.Dispose();

        private void ReturnToMainMenu()
        {
            if (_sceneNavigator.TryGoTo<MainMenuRoute>() == false)
                Debug.LogWarning($"{nameof(ISceneNavigator)} rejected transition to {nameof(MainMenuRoute)}. " +
                                 "Transition is probably already running.");
        }
    }
}