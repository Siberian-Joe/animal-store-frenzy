using NewCore.Bootstrap;
using NewCore.Factories;
using NewCore.Services;
using NewCore.Services.Addressables;
using NewCore.Services.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace NewCore.Installers
{
    public sealed class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _uiRootPrefab;

        public override void InstallBindings()
        {
            Container.Bind<IAddressableResourceLoader>().To<AddressableResourceLoader>().AsSingle();

            Container.Bind<IUIRootLoader>().To<UIRootLoader>().AsSingle();

            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IDataStorage>().To<PlayerPrefsDataStorage>().AsSingle();
            Container.Bind<IViewModelFactory>().To<ViewModelFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameDataService>().AsSingle();

            Container.BindInterfacesAndSelfTo<ApplicationBootstrapper>().AsSingle();
        }
    }
}