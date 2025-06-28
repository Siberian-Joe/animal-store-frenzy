using NewCore.Bootstrap;
using NewCore.Commands;
using NewCore.Factories;
using NewCore.Services.EntityTypeRegistry;
using NewCore.Services.Lifecycle;
using NewCore.ViewBinders;
using NewCore.ViewBinders.Decorators;
using NewCore.Views.World;

namespace NewCore.Installers
{
    public sealed class CoreInstaller : BootstrapperInstaller<CoreBootstrapper>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICommandProcessor>()
                .To<CommandProcessor>()
                .AsSingle();

            Container
                .Bind<IViewModelFactory>()
                .To<ViewModelFactory>()
                .AsSingle();

            Container
                .Bind<IViewFactory>()
                .To<ViewFactory>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerService>()
                .AsSingle();

            Container
                .Bind<ICustomerLifecycle>()
                .To<CustomerLifecycle>()
                .AsSingle();

            Container
                .Bind<INodeView>()
                .FromComponentsInHierarchy()
                .AsTransient();

            Container
                .Bind<IViewBinder>()
                .To<ViewBinder>()
                .AsSingle();

            Container
                .Decorate<IViewBinder>()
                .With<SceneAwareViewBinderDecorator>();

            Container
                .Bind<ISceneNodeComposer>()
                .To<SceneNodeComposer>()
                .AsSingle();

            // TODO: Replace registration of new types in the system with a command
            Container
                .Bind<IEntityTypeRegistry>()
                .To<EntityTypeRegistry>()
                .AsSingle();

            base.InstallBindings();
        }
    }
}