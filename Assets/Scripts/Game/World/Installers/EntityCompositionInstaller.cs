using Game.World.Commands;
using Game.World.EntityRuntime;
using Game.World.Interactions.Shelf;
using Zenject;

namespace Game.World.Installers
{
    public sealed class EntityCompositionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IEntityStateStore>()
                .To<InMemoryEntityStateStore>()
                .AsSingle();

            Container
                .Bind<EntityRoot>()
                .FromComponentsInHierarchy()
                .AsCached();

            Container
                .Bind<EntityActivator>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<RestockShelfCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<TakeFromShelfCommandHandler>()
                .AsSingle();

            Container
                .Bind<GameCommandDispatcher>()
                .AsSingle();

            Container
                .BindInterfacesTo<SceneEntityBootstrap>()
                .AsSingle();
        }
    }
}
