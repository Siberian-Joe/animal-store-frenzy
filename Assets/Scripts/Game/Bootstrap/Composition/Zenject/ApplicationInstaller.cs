using Game.Bootstrap.Startup;
using Game.Presentation.Startup;
using Game.Startup.Composition.Zenject;
using Modules.Startup.Runtime;
using Modules.Startup.Runtime.Contracts;

namespace Game.Bootstrap.Composition.Zenject
{
    public sealed class ApplicationInstaller : StartupInstaller<ApplicationStartup, IApplicationStartupTask>
    {
        protected override void InstallStartupBindings()
        {
            BindStartupTask<PrepareApplicationPresentationStartupTask>();
            BindStartupTask<GoToInitialSceneStartupTask>();
        }
    }
}