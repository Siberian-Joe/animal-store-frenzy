using Modules.Startup.Runtime;
using Modules.Startup.Runtime.Contracts;
using Zenject;

namespace Game.Startup.Composition.Zenject
{
    public abstract class StartupInstaller<TStartup, TTask> : MonoInstaller
        where TStartup : class, IStartup
        where TTask : IStartupTask
    {
        public sealed override void InstallBindings()
        {
            BindStartupInfrastructure();
            InstallStartupBindings();
        }

        protected virtual void BindStartupInfrastructure()
        {
            Container
                .Bind<IStartupTaskDescriptorResolver>()
                .To<StartupTaskDescriptorResolver>()
                .AsSingle();

            Container
                .Bind<StartupPipeline<TTask>>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<TStartup>()
                .AsSingle();

            Container
                .Bind<StartupRunner<TStartup>>()
                .AsSingle();

            Container
                .BindInterfacesTo<StartupRunnerEntryPoint<TStartup>>()
                .AsSingle();

            Container
                .BindExecutionOrder<StartupRunnerEntryPoint<TStartup>>(1000);
        }

        protected abstract void InstallStartupBindings();

        protected void BindStartupTask<TConcreteTask>()
            where TConcreteTask : class, TTask
        {
            Container
                .Bind<TConcreteTask>()
                .AsSingle();

            Container
                .Bind<TTask>()
                .To<TConcreteTask>()
                .FromResolve();
        }
    }
}