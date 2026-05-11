using Zenject;

namespace Game.World.Commands
{
    public sealed class CommandsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (Container.HasBinding<GameCommandDispatcher>())
                return;

            Container
                .Bind<GameCommandDispatcher>()
                .AsSingle();
        }
    }
}