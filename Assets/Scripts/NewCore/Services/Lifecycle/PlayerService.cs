using System;
using NewCore.Commands;
using NewCore.Modules.Interaction.Abstractions;
using NewCore.Services.Input;
using NewCore.ViewModels.World;
using R3;
using UnityEngine;
using Zenject;

namespace NewCore.Services.Lifecycle
{
    public class PlayerService : IPlayerService, IInitializable, IDisposable
    {
        public ReactiveProperty<PlayerViewModel> Player { get; } = new();

        private readonly CompositeDisposable _disposables = new();
        private readonly ICommandProcessor _commandProcessor;
        private readonly IPlayerInputService _inputService;

        private IInteractable _pendingInteraction;
        private Vector2 _pendingTargetPosition;
        private bool _isPendingInteraction;

        public PlayerService(
            ICommandProcessor commandProcessor,
            IPlayerInputService inputService)
        {
            _commandProcessor = commandProcessor;
            _inputService = inputService;
        }

        public void Initialize()
        {
            Player.AddTo(_disposables);

            _inputService.Clicked
                         .Subscribe(OnClicked)
                         .AddTo(_disposables);

            Player
                .Where(viewModel => viewModel != null)
                .Subscribe(viewModel =>
                {
                    viewModel.Arrived
                             .Subscribe(_ => CompleteInteraction())
                             .AddTo(_disposables);
                })
                .AddTo(_disposables);
        }

        private void OnClicked(ClickContext context)
        {
            if (context.IsInteractable)
            {
                _pendingInteraction = context.Interactable;
                _pendingTargetPosition = context.WorldPosition;
                _isPendingInteraction = true;

                if (TryMovePlayer(_pendingTargetPosition))
                    return;

                Debug.LogWarning("Cannot move to interactable! Interaction cancelled.");
                ResetPending();
            }
            else
            {
                ResetPending();

                if (TryMovePlayer(context.WorldPosition))
                    return;

                Debug.LogWarning("Cannot move to this point!");
            }
        }

        public bool TryMovePlayer(Vector2 targetPosition) =>
            _commandProcessor.TryProcess(new MovePlayerCommand(targetPosition));

        private void CompleteInteraction()
        {
            if (_isPendingInteraction == false || _pendingInteraction == null)
                return;

            Player.Value.Interact.Execute(_pendingInteraction);
            ResetPending();
        }

        public void Dispose() => _disposables.Dispose();

        private void ResetPending()
        {
            _pendingInteraction = null;
            _isPendingInteraction = false;
        }
    }
}