using Core.ViewModels;
using Core.ViewModels.MovementStrategies;
using Interfaces.Interactions;
using UniRx;
using UnityEngine;

namespace Core.Views
{
    public class CharacterView : MovableEntityView<CharacterViewModel>
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _interactionRange = 0.5f;

        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _direction = Animator.StringToHash("Direction");

        private Animator _animator;
        private Collider2D[] _results = new Collider2D[10];

        protected override void Initialize()
        {
            base.Initialize();

            _animator = GetComponent<Animator>();

            ViewModel.Direction
                .Subscribe(UpdateAnimatorDirection)
                .AddTo(Disposable);

            ViewModel.SetSpeed(_speed);
            ViewModel.SetMovementStrategy(new PlayerInputMovementStrategy(transform, ViewModel.Speed));

            ViewModel.Interact
                .Subscribe(_ =>
                {
                    var interactable = FindNearestInteractable();
                    interactable?.Interact<IInteraction>(interaction => { interaction.Interact(); });
                })
                .AddTo(Disposable);

            ViewModel.IsMoving
                .Subscribe(isMoving => { _animator.SetBool(_isMoving, isMoving); })
                .AddTo(Disposable);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                var interactable = FindNearestInteractable();
                interactable?.Interact<IShelfInteraction>(shelf => { shelf.RemoveItem(1); });
            }
        }

        private IInteractable FindNearestInteractable()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRange, _results);
            for (var i = 0; i < size; i++)
            {
                if (_results[i].TryGetComponent(out IInteractable interactable))
                {
                    return interactable;
                }
            }

            return null;
        }

        private void UpdateAnimatorDirection(Vector2 direction)
        {
            var directionIndex = ViewModel.CalculateDirectionIndex(direction);

            _animator.SetInteger(_direction, directionIndex);
        }
    }
}