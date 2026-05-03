using R3;
using UnityEngine;
using UnityEngine.UI;
using Screen = Modules.Presentation.Runtime.Panels.Screen;

namespace Game.Presentation.Screens.Core
{
    public sealed class CoreScreen : Screen
    {
        [SerializeField] private Button _mainMenuButton;

        public Observable<Unit> MainMenuRequested => _mainMenuButton.onClick.AsObservable();
    }
}