using System;
using Modules.Presentation.Runtime.Root;
using UnityEngine;
using Zenject;

namespace Game.Presentation.Composition.Zenject
{
    public sealed class BootstrapPresentationInstaller : MonoInstaller
    {
        [SerializeField] private PresentationRoot _presentationRoot;

        public override void InstallBindings()
        {
            if (_presentationRoot == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(BootstrapPresentationInstaller)} requires assigned {nameof(PresentationRoot)}.");
            }

            Container
                .BindInterfacesTo<BootstrapPresentationRootPublisher>()
                .AsSingle()
                .WithArguments(_presentationRoot);
        }
    }
}