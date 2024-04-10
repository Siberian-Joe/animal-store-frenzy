using Core.ViewModels;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Views
{
    public class CustomerView : MovableAndInteractableEntityView<CustomerViewModel>
    {
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private Vector3 _targetPosition;

        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _direction = Animator.StringToHash("Direction");

        private Animator _animator;
        private NavMeshAgent _agent;

        protected override void Initialize()
        {
            base.Initialize();

            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = _speed;
            ViewModel.SetAgent(_agent);

            ViewModel.SetSpeed(_speed);

            ViewModel.Direction.Subscribe(UpdateAnimatorDirection)
                .AddTo(Disposable);

            ViewModel.IsMoving
                .Subscribe(isMoving => { _animator.SetBool(_isMoving, isMoving); })
                .AddTo(Disposable);
        }

        private void UpdateAnimatorDirection(Vector2 direction)
        {
            var directionIndex = ViewModel.CalculateDirectionIndex(direction);

            _animator.SetInteger(_direction, directionIndex);
        }
    }
}