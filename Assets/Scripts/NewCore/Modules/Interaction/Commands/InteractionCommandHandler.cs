using System.Linq;
using NewCore.Commands;

namespace NewCore.Modules.Interaction.Commands
{
    public sealed class InteractionCommandHandler : ICommandHandler<InteractionCommand>
    {
        public bool Handle(InteractionCommand command)
        {
            var context = command.Context;
            return context.Initiator.Rules.Any(rule => rule.TryApply(context));
        }
    }
}