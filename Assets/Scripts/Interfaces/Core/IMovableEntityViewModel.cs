using R3;
using UnityEngine;

namespace Interfaces.Core
{
    public interface IMovableEntityViewModel : IEntityViewModel
    {
        ReadOnlyReactiveProperty<float> Speed { get; }
        ReadOnlyReactiveProperty<Vector2> TargetPosition { get; }
        ReadOnlyReactiveProperty<bool> IsMoving { get; }
        IMovementStrategy MovementStrategy { get; }
        float DistanceThreshold{ get; }
        bool IsMovingToDirection { get; }
        void SetMovementStrategy(IMovementStrategy movementStrategy);
        void UpdateDirection(Vector2 input);
        void SetSpeed(float speed);
    }
}