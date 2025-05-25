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
        [SerializeField] private WorldView _worldView;

        public override void InstallBindings()
        {
            Container
                .Bind<ICommandProcessor>()
                .To<CommandProcessor>().AsSingle();

            Container
                .Bind<ICustomerLifecycle>()
                .To<CustomerLifecycle>().AsSingle();

            base.InstallBindings();

            // TODO: Need to do this separately. Just a placeholder
            _worldView.Bind(new WorldViewModel(Container.Resolve<ICustomerLifecycle>()));
        }
    }
}