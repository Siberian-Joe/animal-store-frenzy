using System;
using Game.World.PlayerInteraction.Navigation;

namespace Game.World.PlayerInteraction
{
    public sealed class PlayerMovementClickHandler : IPlayerClickHandler
    {
        private readonly IPlayerControlledActorContext _actorContext;
        private readonly IPendingPlayerInteraction _pendingInteraction;
        private readonly INavigationTargetResolver _navigationTargetResolver;

        public PlayerMovementClickHandler(
            IPlayerControlledActorContext actorContext,
            IPendingPlayerInteraction pendingInteraction,
            INavigationTargetResolver navigationTargetResolver)
        {
            _actorContext = actorContext ?? throw new ArgumentNullException(nameof(actorContext));
            _pendingInteraction = pendingInteraction ?? throw new ArgumentNullException(nameof(pendingInteraction));
            _navigationTargetResolver = navigationTargetResolver ??
                                        throw new ArgumentNullException(nameof(navigationTargetResolver));
        }

        public int Order => 1000;

        public PlayerClickHandlingResult Handle(PlayerWorldClick click)
        {
            if (_navigationTargetResolver.TryResolve(click.WorldPosition, out var targetPosition) == false)
                return PlayerClickHandlingResult.Consume("Cannot move there.");

            _pendingInteraction.Clear();
            _actorContext.Navigation.SetTarget(targetPosition);
            return PlayerClickHandlingResult.Consume();
        }
    }
}