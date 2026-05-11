namespace Game.World.Commands
{
    public interface IGameCommandPostProcessor
    {
        void Process(IGameCommand command);
    }
}