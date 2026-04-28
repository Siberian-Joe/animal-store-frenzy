using System;
using Game.Presentation.Startup;
using Modules.Presentation.Configuration;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Modules.Presentation.Composition.Zenject
{
    public sealed class PresentationStartupInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _resourcesConfigReference;

        public override void InstallBindings()
        {
            if (_resourcesConfigReference == null || _resourcesConfigReference.RuntimeKeyIsValid() == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(PresentationStartupInstaller)} requires a valid {nameof(PresentationResourcesConfig)} reference.");
            }

            Container
                .BindInterfacesAndSelfTo<LoadPresentationResourcesStartupTask>()
                .AsSingle()
                .WithArguments(_resourcesConfigReference);
        }
    }
}