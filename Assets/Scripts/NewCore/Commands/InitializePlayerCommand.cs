using NewCore.Data;

namespace NewCore.Commands
{
    public class InitializePlayerCommand : ICommand
    {
        public Player Player { get; }

        public InitializePlayerCommand(Player player) => Player = player;
    }
}