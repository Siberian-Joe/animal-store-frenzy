using Interfaces.Core;
using UniRx;
using UnityEngine;

namespace Core.ViewModels.MovementStrategies
{
    public class PlayerInputMovementStrategy : IMovementStrategy
    {
        private readonly Transform _transform;
        private readonly CompositeDisposable _disposable = new();

        private float _speed;

        public PlayerInputMovementStrategy(Transform transform, IReadOnlyReactiveProperty<float> speed)
        {
            _transform = transform;

            speed
                .Subscribe(SetSpeed)
                .AddTo(_disposable);
        }

        public void Move(Vector2 direction)
        {
            var clampedDirection = Vector2.ClampMagnitude(direction, 1);
            _transform.Translate(clampedDirection * _speed * Time.fixedDeltaTime);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}