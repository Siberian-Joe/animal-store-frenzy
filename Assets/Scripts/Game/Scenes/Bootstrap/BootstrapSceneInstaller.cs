using Game.MainMenu.Integration.Presentation;
using Game.MainMenu.Startup;
using Game.Presentation;
using Game.Presentation.Contracts.Preparation;
using Game.Presentation.Startup;
using Game.Scenes.Shared;

namespace Game.Scenes.Bootstrap
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