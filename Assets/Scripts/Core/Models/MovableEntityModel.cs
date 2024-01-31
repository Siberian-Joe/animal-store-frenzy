using UniRx;
using UnityEngine;

namespace Core.Enums
{
    public abstract class MovableEntityModel : EntityModel
    {
        public ReactiveProperty<float> Speed { get; } = new();
        public ReactiveProperty<Vector2> TargetPosition { get; } = new(Vector2.zero);
        public ReactiveProperty<bool> IsMoving { get; } = new(false);
    }
}