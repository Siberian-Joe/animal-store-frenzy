using UnityEngine;

namespace NewCore.Commands
{
    public sealed class MovePlayerCommand : ICommand
    {
        public Vector2 TargetPosition { get; }

        public MovePlayerCommand(Vector2 targetPosition) => TargetPosition = targetPosition;
    }
}