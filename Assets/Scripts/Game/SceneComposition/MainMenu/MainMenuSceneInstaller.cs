using Game.Presentation.Screens.MainMenu;
using Game.Presentation.Screens.MainMenu.Startup;
using Game.Presentation.Startup;
using Game.SceneComposition.Shared;
using Modules.Presentation.Runtime.Contracts.Preparation;

namespace Game.SceneComposition.MainMenu
{
    public sealed class MainMenuSceneInstaller : SceneStartupInstaller
    {
        protected override void InstallSceneBindings()
        {
            Container
                .Bind<IPanelPreparationPlan>()
                .To<MainMenuPresentationPlan>()
                .AsSingle();

            BindStartupTask<PrepareConfiguredPanelsStartupTask>();
            BindStartupTask<OpenMainMenuScreenStartupTask>();
        }
    }
}