using Zenject;

namespace Game.World.Commands
{
    public sealed class CommandsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<GameCommandDispatcher>()
                .AsSingle();
        }
    }
}