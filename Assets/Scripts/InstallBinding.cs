using Core.ViewModels;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using Services;
using Services.DataServices;
using Zenject;

public class InstallBinding : MonoInstaller
{
    public override void InstallBindings()
    {
        // // Регистрация модели
        // Container.Bind<MainMenuModel>().AsTransient();
        //
        // // Регистрация ViewModel
        // Container.Bind<IViewModel<IModel>>().To<MainMenuViewModel>().AsTransient();
        //
        // // Регистрация ViewModelFactory
        // Container.Bind<IViewModelFactory>().To<ViewModelFactory>().AsCached();
        //
        // // Регистрация UIManager
        // Container.Bind<IUIManager>().To<UIManager>().AsSingle();

        Container.Bind<IInputService>().To<InputService>().AsSingle();
        Container.Bind<IInteractableEntitiesLocatorService>().To<InteractableEntitiesLocatorService>().AsSingle();
        Container.Bind<ILoggingService>().To<LoggingService>().AsSingle();

        Container.Bind<IStorageStrategy>().To<PlayerPrefsStorageStrategy>().AsSingle();
        Container.Bind<IDataService>().To<DataService>().AsSingle();

        Container.BindInterfacesTo<InteractiveObjectLocationService>().AsSingle();

        Container.Bind<CharacterViewModel>().AsTransient();
        Container.Bind<CameraViewModel>().AsTransient();
        Container.Bind<ChestEntityViewModel>().AsTransient();
        Container.Bind<ShelfEntityViewModel>().AsTransient();
        Container.Bind<CustomerViewModel>().AsTransient();
        //
        // Container.Bind<EnemyViewModel>().AsTransient();
        //
        // Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
    }
}