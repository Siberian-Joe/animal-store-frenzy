using NewCore.Bootstrap;
using NewCore.Commands;
using NewCore.Services.Lifecycle;
using NewCore.ViewModels;
using NewCore.Views.World;
using UnityEngine;

namespace NewCore.Installers
{
    public sealed class CoreInstaller : BootstrapperInstaller<CoreBootstrapper>
    {
        [SerializeField] private WorldBinder _worldBinder;

        public override void InstallBindings()
        {
            Container.Bind<ICommandProcessor>().To<CommandProcessor>().AsSingle();
            Container.Bind<ICustomerLifecycle>().To<CustomerLifecycle>().AsSingle();

            base.InstallBindings();

            _worldBinder.Bind(new WorldViewModel(Container.Resolve<ICustomerLifecycle>())); // TODO: Need to do this separately. Just a placeholder
        }
    }
}