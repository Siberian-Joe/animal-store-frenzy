using System;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;

namespace Game.World.PlayerInteraction
{
    public sealed class PlayerInteractionClickHandler : IPlayerClickHandler
    {
        private readonly IPlayerControlledActorContext _actorContext;
        private readonly IPendingPlayerInteraction _pendingInteraction;

        public PlayerInteractionClickHandler(
            IPlayerControlledActorContext actorContext,
            IPendingPlayerInteraction pendingInteraction)
        {
            _actorContext = actorContext ?? throw new ArgumentNullException(nameof(actorContext));
            _pendingInteraction = pendingInteraction ?? throw new ArgumentNullException(nameof(pendingInteraction));
        }

        public int Order => 0;

        public PlayerClickHandlingResult Handle(PlayerWorldClick click)
        {
            if (click.HasHit == false || click.HitComponent == false)
                return PlayerClickHandlingResult.Pass();

            var targetRoot = click.HitComponent.GetComponentInParent<EntityRoot>();
            if (targetRoot == false || targetRoot.TryFindOwnedComponent(out InteractionTargetPart target) == false ||
                target == false)
                return PlayerClickHandlingResult.Pass();

            if (target.TryGetPrimaryOption(_actorContext.InteractionActor, out var option) == false)
                return PlayerClickHandlingResult.Consume("No available interaction.");

            _pendingInteraction.Set(option);
            _actorContext.Navigation.SetTarget(option.ApproachPoint);
            return PlayerClickHandlingResult.Consume();
        }
    }
}