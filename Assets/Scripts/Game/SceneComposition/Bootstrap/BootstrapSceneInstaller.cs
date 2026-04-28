using Game.Presentation.Screens.MainMenu;
using Game.Presentation.Screens.MainMenu.Startup;
using Game.Presentation.Startup;
using Game.SceneComposition.Shared;
using Modules.Presentation.Contracts.Preparation;

namespace Game.SceneComposition.Bootstrap
{
    public class BootstrapSceneInstaller : SceneStartupInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container
                .Bind<IPanelPreparationPlan>()
                .To<MainMenuPresentationPlan>()
                .AsSingle();

            BindStartupTask<PrepareConfiguredPanelsStartupTask>();
            BindStartupTask<OpenMainMenuScreenStartupTask>();
        }
    }
}