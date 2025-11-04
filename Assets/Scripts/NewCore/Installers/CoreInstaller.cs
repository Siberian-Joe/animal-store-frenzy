using NewCore.Bootstrap;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Factories;
using NewCore.Modules.Interaction;
using NewCore.Modules.Interaction.Abstractions;
using NewCore.Modules.Interaction.Rules;
using NewCore.Services.EntityTypeRegistry;
using NewCore.Services.GameData;
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
                .Rebind<IProxyFactory>()
                .To<ProxyFactory>()
                .AsSingle();

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
                .BindInterfacesAndSelfTo<ShelvesLifecycle>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<CustomerLifecycle>()
                .AsSingle();

            Container
                .Bind<INodeView>()
                .FromComponentsInHierarchy()
                .AsTransient();

            Container
                .Bind<IViewBinder>()
                .To<ViewBinder>()
                .AsSingle();

            // TODO: Think of a way to ensure the decorator correctly calls Dispose
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

            Container
                .Bind<IInteractionRule>()
                .To<StoreOnShelfRule>()
                .AsSingle()
                .WhenInjectedInto<Player>();

            Container.Decorate<IGameDataService>()
                     .With<RuntimeScopedGameDataService>();

            base.InstallBindings();
        }
    }
}