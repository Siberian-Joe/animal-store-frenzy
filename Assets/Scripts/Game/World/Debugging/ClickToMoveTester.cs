using Game.World.Core;
using Game.World.Navigation;
using NewCore.Services.Input;
using R3;
using UnityEngine;
using Zenject;

namespace Game.World.Debugging
{
    public sealed class ClickToMoveInputTester : MonoBehaviour
    {
        [SerializeField] private EntityRoot _targetEntity;

        private readonly CompositeDisposable _disposables = new();

        private IPlayerInputService _playerInputService;

        [Inject]
        public void Construct(IPlayerInputService playerInputService)
        {
            _playerInputService = playerInputService;
        }

        private void Start()
        {
            if (_playerInputService == null)
            {
                Debug.LogError($"{nameof(ClickToMoveInputTester)}: {nameof(IPlayerInputService)} was not injected.",
                    this);
                return;
            }

            if (_targetEntity == false)
            {
                Debug.LogError($"{nameof(ClickToMoveInputTester)}: target entity is not assigned.", this);
                return;
            }

            _playerInputService.Clicked
                .Subscribe(HandleClick)
                .AddTo(_disposables);
        }

        private void HandleClick(ClickContext click)
        {
            if (_targetEntity.TryGetFeature<INavigationFeature>(out var navigation) == false)
                return;

            navigation.SetTarget(click.WorldPosition);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
