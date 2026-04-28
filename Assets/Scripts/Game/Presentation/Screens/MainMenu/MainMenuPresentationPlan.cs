using Modules.Presentation.Runtime.Preparation;

namespace Game.Presentation.Screens.MainMenu
{
    public sealed class MainMenuPresentationPlan : PanelPreparationPlan
    {
        public MainMenuPresentationPlan() => Add<MainMenuScreenPresenter>();
    }
}