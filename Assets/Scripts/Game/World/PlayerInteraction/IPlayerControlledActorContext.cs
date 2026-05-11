using Game.World.EntityRuntime;
using Game.World.Features.Navigation;
using Game.World.Interactions;

namespace Game.World.PlayerInteraction
{
    public interface IPlayerControlledActorContext
    {
        EntityRoot Root { get; }
        INavigationFeature Navigation { get; }
        IInteractionActor InteractionActor { get; }
    }
}