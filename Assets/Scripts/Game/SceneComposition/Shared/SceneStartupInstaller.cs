using Modules.SceneReady.Contracts;
using Modules.SceneReady.Runtime;
using Modules.Startup.Contracts;
using Modules.Startup.Runtime;
using Zenject;

namespace Game.SceneComposition.Shared
{
    public abstract class SceneStartupInstaller : MonoInstaller
    {
        public override void Start()
        {
            base.Start();

            var runner = Container.Resolve<StartupRunner<SceneStartup>>();
            runner.Initialize();
        }

        public override void InstallBindings()
        {
            Container
                .Bind<ISceneReadyGate>()
                .To<SceneReadyGate>()
                .AsSingle();

            Container
                .Bind<IStartupTaskDescriptorResolver>()
                .To<StartupTaskDescriptorResolver>()
                .AsSingle();

            Container
                .Bind<StartupPipeline>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<SceneStartup>()
                .AsSingle();

            Container
                .Bind<StartupRunner<SceneStartup>>()
                .AsSingle();
        }

        protected void BindStartupTask<TTask>()
            where TTask : class, IStartupTask =>
            Container
                .BindInterfacesAndSelfTo<TTask>()
                .AsSingle();
    }
}