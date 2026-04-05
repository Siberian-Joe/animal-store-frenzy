using Game.World.Composition;
using Zenject;

namespace Game.World.ProductContainer
{
    public class ProductContainerModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICompositionModule>()
                .To<ProductContainerModule>()
                .AsSingle();
        }
    }
}
