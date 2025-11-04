using NewCore.Modules.Interaction.Context;

namespace NewCore.Modules.Interaction.Abstractions
{
    public interface IInteractionRule
    {
        bool TryApply(InteractionContext context);
    }
}