using System;
using Game.Presentation.Transitions.Loading;
using Game.SceneNavigation.Startup;
using Modules.SceneNavigation.Runtime;
using Modules.SceneNavigation.Runtime.Catalogs;
using Modules.SceneNavigation.Runtime.Contracts;
using Modules.SceneNavigation.Runtime.Loaders;
using Modules.SceneNavigation.Runtime.Navigators;
using Modules.Startup.Contracts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Game.SceneNavigation.Composition.Zenject
{
    public sealed class SceneNavigationInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _configReference;

        public override void InstallBindings()
        {
            if (_configReference == null || _configReference.RuntimeKeyIsValid() == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(SceneNavigationInstaller)} requires valid scene navigation config reference.");
            }

            Container
                .BindInstance(_configReference)
                .WhenInjectedInto<LoadSceneNavigationConfigStartupTask>();

            Container
                .BindInterfacesAndSelfTo<SceneCatalog>()
                .AsSingle();

            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<SceneReadinessCoordinator>()
                .AsSingle();

            Container
                .Bind<ISceneTransitionScreen>()
                .To<LoadingOverlaySceneTransitionScreen>()
                .AsSingle();

            Container
                .Bind<IScenePresentationCleaner>()
                .To<PanelScenePresentationCleaner>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<SceneNavigator>()
                .AsSingle();

            Container
                .Bind<LoadSceneNavigationConfigStartupTask>()
                .AsSingle();

            Container
                .Bind<IApplicationStartupTask>()
                .To<LoadSceneNavigationConfigStartupTask>()
                .FromResolve();
        }
    }
}