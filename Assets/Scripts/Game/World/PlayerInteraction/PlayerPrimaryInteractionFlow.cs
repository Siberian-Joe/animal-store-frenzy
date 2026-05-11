using System;
using System.Collections.Generic;
using R3;
using Zenject;

namespace Game.World.PlayerInteraction
{
    public sealed class PlayerPrimaryInteractionFlow : IInitializable, IDisposable
    {
        private readonly IPlayerWorldInput _input;
        private readonly IPlayerFeedback _feedback;
        private readonly List<IPlayerClickHandler> _handlers;
        private readonly CompositeDisposable _disposables = new();

        public PlayerPrimaryInteractionFlow(
            IPlayerWorldInput input,
            IPlayerFeedback feedback,
            List<IPlayerClickHandler> handlers)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _handlers.Sort(static (left, right) => left.Order.CompareTo(right.Order));
        }

        public void Initialize()
        {
            _input.Clicked
                .Subscribe(HandleClick)
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();

        private void HandleClick(PlayerWorldClick click)
        {
            foreach (var handler in _handlers)
            {
                var result = handler.Handle(click);
                if (result.HasFeedback)
                    _feedback.ShowMessage(result.FeedbackMessage);

                if (result.IsConsumed)
                    return;
            }
        }
    }
}