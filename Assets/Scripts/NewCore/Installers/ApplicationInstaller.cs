using NewCore.Bootstrap;
using NewCore.Factories;
using NewCore.Modules.Interaction;
using NewCore.Services;
using NewCore.Services.GameData;
using NewCore.Services.Input;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.Scenes;
using NewCore.Services.Storage;
using NewCore.Services.UI;
using NewCore.Services.UI.Factories;
using NewCore.Services.UI.Registries;
using NewCore.ViewModels.UI;
using NewCore.ViewModels.World;
using UnityEngine;
using Zenject;

namespace NewCore.Installers
{
    public sealed class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private CameraProvider _cameraProviderPrefab;

        public override void InstallBindings()
        {
            Container
                .Bind<IProxyFactory>()
                .To<ProxyFactory>()
                .AsSingle();

            Container
                .Bind<IResourceLoader>()
                .To<ResourceLoader>()
                .AsSingle();

            Container
                .Bind<IPanelCache>()
                .To<PanelCache>()
                .AsSingle();

            Container
                .Bind<IScreenRegistry>()
                .To<ScreenRegistry>()
                .AsSingle();

            Container
                .Bind<IOverlayRegistry>()
                .To<OverlayRegistry>()
                .AsSingle();

            Container
                .Bind<ISystemOverlayRegistry>()
                .To<SystemOverlayRegistry>()
                .AsSingle();

            Container
                .Bind<IPanelHandlerResolver>()
                .To<ScreenHandlerResolver>()
                .AsSingle();

            Container
                .Bind<IPanelHandlerResolver>()
                .To<OverlayHandlerResolver>()
                .AsSingle();

            Container
                .Bind<IPanelHandlerResolver>()
                .To<SystemOverlayHandlerResolver>()
                .AsSingle();

            Container
                .Bind<IPanelHandlerResolver>()
                .To<DefaultHandlerResolver>()
                .AsSingle();

            Container
                .Bind<IPanelHandlerFactory>()
                .To<PanelHandlerFactory>()
                .AsSingle();

            Container
                .Bind<IPanelService>()
                .To<PanelService>()
                .AsSingle();

            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();

            Container
                .Bind<IViewModelFactory>()
                .To<ViewModelFactory>()
                .AsSingle();

            Container
                .Bind<IStorage>()
                .To<PlayerPrefsStorage>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<GameDataService>()
                .AsSingle();

            Container
                .Bind<LoadingSystemOverlayViewModel>()
                .AsTransient();

            Container
                .Bind<MainMenuScreenViewModel>()
                .AsTransient();

            Container
                .Bind<CoreScreenViewModel>()
                .AsTransient();

            Container
                .Bind<WorldViewModel>()
                .AsTransient();

            Container.Bind<ICameraProvider>()
                .FromComponentInNewPrefab(_cameraProviderPrefab)
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<PlayerInputService>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ApplicationBootstrapper>()
                .AsSingle();
        }
    }
}