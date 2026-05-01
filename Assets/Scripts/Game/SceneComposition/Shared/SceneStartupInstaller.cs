using Game.SceneComposition.Shared.Startup;
using Game.Startup.Composition.Zenject;
using Modules.Readiness.Runtime;
using Modules.Readiness.Runtime.Contracts;
using Modules.Startup.Contracts;
using Modules.Startup.Runtime;
using UnityEngine;

namespace Game.SceneComposition.Shared
{
    public abstract class SceneStartupInstaller : StartupInstaller<SceneStartup, ISceneStartupTask>
    {
        protected override void BindStartupInfrastructure()
        {
            base.BindStartupInfrastructure();

            Container
                .Bind<ISceneReadyGate>()
                .To<SceneReadyGate>()
                .AsSingle();

            BindSceneReadinessPublicationIfAvailable();
        }

        protected sealed override void InstallStartupBindings()
        {
            BindStartupTask<WaitApplicationStartupTask>();

            InstallSceneBindings();
        }

        protected virtual void InstallSceneBindings()
        {
        }

        private void BindSceneReadinessPublicationIfAvailable()
        {
            if (Container.HasBinding<ISceneReadinessPublisher>() == false)
                return;

            Container
                .Bind<GameObject>()
                .FromInstance(gameObject)
                .WhenInjectedInto<SceneReadinessPublication>();

            Container
                .BindInterfacesTo<SceneReadinessPublication>()
                .AsSingle();
        }
    }
}