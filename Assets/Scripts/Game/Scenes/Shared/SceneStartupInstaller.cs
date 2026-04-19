using Game.SceneReady.Contracts;
using Game.SceneReady.Runtime;
using Game.Startup.Contracts;
using Game.Startup.Runtime;
using Zenject;

namespace Game.Scenes.Shared
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

        protected void BindStartupTask<TTask>() where TTask : class, IStartupTask =>
            Container
                .Bind<IStartupTask>()
                .To<TTask>()
                .AsSingle();
    }
}