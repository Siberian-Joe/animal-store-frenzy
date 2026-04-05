using Game.World.Composition;
using Zenject;

namespace Game.World.InteractionTarget
{
    public class InteractionTargetModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICompositionModule>()
                .To<InteractionTargetModule>()
                .AsSingle();
        }
    }
}
