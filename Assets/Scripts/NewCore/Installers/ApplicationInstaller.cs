using NewCore.Bootstrap;
using NewCore.Factories;
using NewCore.Services;
using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;
using Zenject;

namespace NewCore.Installers
{
    public sealed class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var prefabUIRoot =
                Resources.Load<UIRoot>("UIRoot"); // TODO: Implement loading from resources using Addressables
            var uiRoot = Instantiate(prefabUIRoot); // TODO: Implement UI management using a separate service

            DontDestroyOnLoad(uiRoot.gameObject);

            Container.Bind<UIRoot>().FromInstance(uiRoot).AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IDataStorage>().To<PlayerPrefsDataStorage>().AsSingle();
            Container.Bind<IViewModelFactory>().To<ViewModelFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameDataService>().AsSingle();

            Container.BindInterfacesAndSelfTo<ApplicationBootstrapper>().AsSingle();
        }
    }
}