using Modules.Presentation.Contracts.Navigation;
using Modules.Presentation.Contracts.Preparation;
using Modules.Presentation.Infrastructure.Zenject;
using Modules.Presentation.Runtime.Catalog;
using Modules.Presentation.Runtime.Navigation;
using Modules.Presentation.Runtime.Preparation;
using Modules.Presentation.Runtime.Registry;
using Modules.Presentation.Runtime.Root;
using Zenject;

namespace Modules.Presentation.Composition.Zenject
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