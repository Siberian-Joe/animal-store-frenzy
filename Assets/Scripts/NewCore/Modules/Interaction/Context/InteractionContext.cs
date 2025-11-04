using NewCore.Modules.Interaction.Abstractions;

namespace NewCore.Modules.Interaction.Context
{
    public sealed class InteractionContext
    {
        public IActor Initiator { get; }
        public IActor Target { get; }

        public InteractionContext(IActor initiator, IActor target)
        {
            Initiator = initiator;
            Target = target;
        }
    }
}