using Game.World.Interactions;

namespace Game.World.UtilityAi
{
    public interface IInteractionBackedUtilityOption : IUtilityOption
    {
        bool MatchesInteraction(InteractionOption option);
    }
}