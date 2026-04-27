using Game.Presentation.Infrastructure.Zenject;
using Game.Presentation.Runtime.Preparation;
using Game.Presentation.Startup;
using Zenject;

namespace Game.Presentation.Composition.Zenject
{
    public sealed class PresentationScenePreparationScopeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IPanelInstanceFactory>()
                .To<ZenjectPanelInstanceFactory>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PanelLifetimeStore>()
                .AsSingle();

            Container
                .Bind<IPanelPreparationScope>()
                .To<PanelPreparationScope>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PublishPresentationScopeStartupTask>()
                .AsSingle();
        }
    }
}