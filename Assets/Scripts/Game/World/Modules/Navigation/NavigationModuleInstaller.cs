using Game.World.Composition;
using Zenject;

namespace Game.World.Navigation
{
    public class NavigationModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICompositionModule>()
                .To<NavigationModule>()
                .AsSingle();
        }
    }
}
