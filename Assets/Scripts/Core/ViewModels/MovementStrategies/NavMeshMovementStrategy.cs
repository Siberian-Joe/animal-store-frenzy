using Interfaces.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Core.ViewModels.MovementStrategies
{
    public class NavMeshMovementStrategy : IMovementStrategy
    {
        private readonly NavMeshAgent _agent;
        private readonly CompositeDisposable _disposable = new();

        public NavMeshMovementStrategy(NavMeshAgent agent, IReadOnlyReactiveProperty<float> speed)
        {
            _agent = agent;

            speed
                .Subscribe(SetSpeed)
                .AddTo(_disposable);
        }

        public void Move(Vector2 direction)
        {
            _agent.SetDestination(direction);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void SetSpeed(float speed)
        {
            _agent.speed = speed;
        }
    }
}