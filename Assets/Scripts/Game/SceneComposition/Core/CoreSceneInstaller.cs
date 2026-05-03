using Game.Presentation.Screens.Core;
using Game.Presentation.Screens.Core.Startup;
using Game.Presentation.Startup;
using Game.SceneComposition.Shared;
using Modules.Presentation.Runtime.Contracts.Preparation;

namespace Game.SceneComposition.Core
{
    public sealed class CoreSceneInstaller : SceneStartupInstaller
    {
        protected override void InstallSceneBindings()
        {
            Container
                .Bind<IPanelPreparationPlan>()
                .To<CorePresentationPlan>()
                .AsSingle();

            BindStartupTask<PrepareConfiguredPanelsStartupTask>();
            BindStartupTask<OpenCoreScreenStartupTask>();
        }
    }
}