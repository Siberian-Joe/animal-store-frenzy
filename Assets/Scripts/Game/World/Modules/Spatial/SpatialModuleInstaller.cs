using Game.World.Composition;
using Zenject;

namespace Game.World.Spatial
{
    public class SpatialModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICompositionModule>()
                .To<SpatialModule>()
                .AsSingle();
        }
    }
}
