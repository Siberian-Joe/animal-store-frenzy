using Game.World.Composition;
using Zenject;

namespace Game.World.ShelfRestocker
{
    public class ShelfRestockerModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICompositionModule>()
                .To<ShelfRestockerModule>()
                .AsSingle();
        }
    }
}
