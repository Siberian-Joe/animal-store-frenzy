using R3;
using UnityEngine;
using UnityEngine.UI;
using Screen = Modules.Presentation.Runtime.Panels.Screen;

namespace Game.Presentation.Screens.MainMenu
{
    public sealed class MainMenuScreen : Screen
    {
        [SerializeField] private Button _playButton;

        public Observable<Unit> PlayRequested => _playButton.onClick.AsObservable();
    }
}