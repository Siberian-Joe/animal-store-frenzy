using NewCore.Bootstrap;
using NewCore.Factories;
using NewCore.Services;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.UI;
using NewCore.Services.UI.Factories;
using NewCore.Services.UI.Registries;
using Zenject;

namespace NewCore.Installers
{
    public sealed class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IResourceLoader>().To<ResourceLoader>().AsSingle();

            Container.Bind<IPanelCache>().To<PanelCache>().AsSingle();
            Container.Bind<IScreenRegistry>().To<ScreenRegistry>().AsSingle();
            Container.Bind<IOverlayRegistry>().To<OverlayRegistry>().AsSingle();
            Container.Bind<ISystemOverlayRegistry>().To<SystemOverlayRegistry>().AsSingle();

            Container.Bind<IPanelHandlerResolver>().To<ScreenHandlerResolver>().AsSingle();
            Container.Bind<IPanelHandlerResolver>().To<OverlayHandlerResolver>().AsSingle();
            Container.Bind<IPanelHandlerResolver>().To<SystemOverlayHandlerResolver>().AsSingle();
            Container.Bind<IPanelHandlerResolver>().To<DefaultHandlerResolver>().AsSingle();

            Container.Bind<IPanelHandlerFactory>().To<PanelHandlerFactory>().AsSingle();
            Container.Bind<IPanelService>().To<PanelService>().AsSingle();

            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IDataStorage>().To<PlayerPrefsDataStorage>().AsSingle();
            Container.Bind<IViewModelFactory>().To<ViewModelFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameDataService>().AsSingle();

            Container.BindInterfacesAndSelfTo<ApplicationBootstrapper>().AsSingle();
        }
    }
}