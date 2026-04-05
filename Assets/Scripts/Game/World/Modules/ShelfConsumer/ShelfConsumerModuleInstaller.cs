using Game.World.Composition;
using Zenject;

namespace Game.World.ShelfConsumer
{
    public class ShelfConsumerModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICompositionModule>()
                .To<ShelfConsumerModule>()
                .AsSingle();
        }
    }
}
