using System;
using Game.Presentation.Startup;
using Modules.Presentation.Runtime.Configuration;
using Modules.Startup.Runtime.Contracts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Game.Presentation.Composition.Zenject
{
    public sealed class PresentationStartupInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _resourcesConfigReference;

        public override void InstallBindings()
        {
            if (_resourcesConfigReference == null || _resourcesConfigReference.RuntimeKeyIsValid() == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(PresentationStartupInstaller)} requires valid {nameof(PresentationResourcesConfig)} reference.");
            }

            Container
                .BindInstance(_resourcesConfigReference)
                .WhenInjectedInto<LoadPresentationResourcesStartupTask>();

            Container
                .Bind<LoadPresentationResourcesStartupTask>()
                .AsSingle();

            Container
                .Bind<IApplicationStartupTask>()
                .To<LoadPresentationResourcesStartupTask>()
                .FromResolve();
        }
    }
}