using Game.Presentation.Contracts.Navigation;
using Game.Presentation.Contracts.Preparation;
using Game.Presentation.Infrastructure.Zenject;
using Game.Presentation.Runtime.Catalog;
using Game.Presentation.Runtime.Navigation;
using Game.Presentation.Runtime.Preparation;
using Game.Presentation.Runtime.Registry;
using Game.Presentation.Runtime.Root;
using Zenject;

namespace Game.Presentation.Composition.Zenject
{
    public sealed class PresentationProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<PanelCatalog>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PanelRootProvider>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PanelRegistry>()
                .AsSingle();

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
                .BindInterfacesAndSelfTo<PanelPreparationScopeProvider>()
                .AsSingle();

            Container
                .Bind<IPanelPreparer>()
                .To<PanelPreparer>()
                .AsSingle();

            Container
                .Bind<PanelNavigationHistory>()
                .AsSingle();

            Container
                .Bind<IPanelNavigator>()
                .To<PanelNavigator>()
                .AsSingle();
        }
    }
}