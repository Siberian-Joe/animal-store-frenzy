using NewCore.Commands;
using NewCore.Modules.Interaction.Context;

namespace NewCore.Modules.Interaction.Commands
{
    public sealed class InteractionCommand : ICommand
    {
        public InteractionContext Context { get; }

        public InteractionCommand(InteractionContext context) => Context = context;
    }
}