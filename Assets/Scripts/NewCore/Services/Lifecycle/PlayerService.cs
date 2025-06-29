using System;
using NewCore.Commands;
using NewCore.Services.Input;
using NewCore.ViewModels;
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

        public PlayerService(
            ICommandProcessor commandProcessor,
            IPlayerInputService inputService)
        {
            _commandProcessor = commandProcessor;
            _inputService = inputService;
        }

        public void Initialize()
        {
            Player
                .AddTo(_disposables);

            _inputService.WorldClicked
                .Subscribe(position =>
                {
                    if (!TryMovePlayer(position))
                        Debug.LogWarning("Failed to move player");
                })
                .AddTo(_disposables);
        }

        public bool TryMovePlayer(Vector2 targetPosition) =>
            _commandProcessor.TryProcess(new MovePlayerCommand(targetPosition));

        public void Dispose() => _disposables.Dispose();
    }
}