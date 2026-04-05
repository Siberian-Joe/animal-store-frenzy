using Game.World.Composition;
using Game.World.Core;
using Zenject;

namespace Game.World.Composition
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
                .Bind<IEntityComposer>()
                .To<EntityComposer>()
                .AsSingle();

            Container
                .BindInterfacesTo<SceneCompositionBootstrap>()
                .AsSingle();
        }
    }
}
