using Game.MainMenu.Presentation;
using Game.Presentation.Runtime.Preparation;

namespace Game.MainMenu.Integration.Presentation
{
    public sealed class MainMenuPresentationPlan : PanelPreparationPlan
    {
        public MainMenuPresentationPlan() => Add<MainMenuScreenPresenter>();
    }
}