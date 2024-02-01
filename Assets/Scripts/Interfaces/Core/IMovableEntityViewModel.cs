using UniRx;
using UnityEngine;

namespace Interfaces.Core
{
    public interface IMovableEntityViewModel : IEntityViewModel
    {
        IReadOnlyReactiveProperty<float> Speed { get; }
        IReadOnlyReactiveProperty<Vector2> TargetPosition { get; }
        IReadOnlyReactiveProperty<bool> IsMoving { get; }
        IMovementStrategy MovementStrategy { get; }
        float DistanceThreshold{ get; }
        bool IsMovingToDirection { get; }
        void SetMovementStrategy(IMovementStrategy movementStrategy);
        void UpdateDirection(Vector2 input);
        void SetSpeed(float speed);
    }
}