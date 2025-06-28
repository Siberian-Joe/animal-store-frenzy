using System;
using NewCore.Commands;
using NewCore.ViewModels;
using R3;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public class PlayerService : IPlayerService, IDisposable
    {
        public ReactiveProperty<PlayerViewModel> Player { get; } = new();

        private readonly ICommandProcessor _commandProcessor;

        public PlayerService(ICommandProcessor commandProcessor) => _commandProcessor = commandProcessor;

        public bool TryMovePlayer(Vector3 targetPosition) =>
            _commandProcessor.TryProcess(new MovePlayerCommand(targetPosition));

        public void Dispose() => Player?.Dispose();
    }
}