using System;
using Game.World.EntityRuntime;
using Game.World.Features.Navigation;
using Game.World.Interactions;

namespace Game.World.PlayerInteraction
{
    public sealed class PlayerControlledActorContext : IPlayerControlledActorContext
    {
        public EntityRoot Root { get; }
        public INavigationFeature Navigation { get; }
        public IInteractionActor InteractionActor { get; }

        public PlayerControlledActorContext(EntityRoot root)
        {
            Root = root ? root : throw new ArgumentNullException(nameof(root));
            Navigation = Root.FindOwnedComponent<INavigationFeature>()
                         ?? throw new InvalidOperationException(
                             $"Player entity '{Root.name}' has no navigation feature.");
            InteractionActor = Root.FindOwnedComponent<IInteractionActor>()
                               ?? throw new InvalidOperationException(
                                   $"Player entity '{Root.name}' has no interaction actor.");
        }
    }
}