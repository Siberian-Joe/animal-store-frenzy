using UniRx;
using UnityEngine;

namespace Core.Models
{
    public abstract class MovableEntityModel : EntityModel
    {
        public ReactiveProperty<float> Speed { get; } = new();
        public ReactiveProperty<Vector2> Direction { get; } = new(Vector2.zero);
        public ReactiveProperty<Vector2> TargetPosition { get; } = new(Vector2.zero);
        public ReactiveProperty<bool> IsMoving { get; } = new(false);
    }
}