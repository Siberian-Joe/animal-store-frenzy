using System;
using Game.World.Commands;
using R3;
using Zenject;

namespace Game.World.PlayerInteraction
{
    public sealed class PlayerInteractionArrivalExecutor : IInitializable, IDisposable
    {
        private readonly IPlayerControlledActorContext _actorContext;
        private readonly IPendingPlayerInteraction _pendingInteraction;
        private readonly GameCommandDispatcher _commandDispatcher;
        private readonly CompositeDisposable _disposables = new();

        public PlayerInteractionArrivalExecutor(
            IPlayerControlledActorContext actorContext,
            IPendingPlayerInteraction pendingInteraction,
            GameCommandDispatcher commandDispatcher)
        {
            _actorContext = actorContext ?? throw new ArgumentNullException(nameof(actorContext));
            _pendingInteraction = pendingInteraction ?? throw new ArgumentNullException(nameof(pendingInteraction));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        }

        public void Initialize()
        {
            _actorContext.Navigation.Arrived
                .Subscribe(_ => ExecutePendingInteraction())
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();

        private void ExecutePendingInteraction()
        {
            if (_pendingInteraction.TryConsume(out var option) == false)
                return;

            _commandDispatcher.Dispatch(option.Command);
        }
    }
}