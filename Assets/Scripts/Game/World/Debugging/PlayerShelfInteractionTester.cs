using Game.World.Core;
using Game.World.Interactions;
using Game.World.Navigation;
using NewCore.Services.Input;
using R3;
using UnityEngine;
using Zenject;

namespace Game.World.Debugging
{
    public sealed class PlayerShelfInteractionTester : MonoBehaviour
    {
        [SerializeField] private EntityRoot _player;

        private readonly CompositeDisposable _disposables = new();

        private IPlayerInputService _inputService;
        private IEntityInteraction _pendingInteraction;
        private INavigationFeature _navigation;

        [Inject]
        public void Construct(IPlayerInputService inputService) => _inputService = inputService;

        private void Start()
        {
            if (_player == false)
            {
                Debug.LogError($"{nameof(PlayerShelfInteractionTester)}: player entity is not assigned", this);
                return;
            }

            if (_player.TryGetFeature(out _navigation) == false)
            {
                Debug.LogError($"{nameof(PlayerShelfInteractionTester)}: player has no navigation feature", this);
                return;
            }

            _navigation.Arrived
                .Subscribe(_ => CompletePendingInteraction())
                .AddTo(_disposables);

            _inputService.Clicked
                .Subscribe(HandleClick)
                .AddTo(_disposables);
        }

        private void HandleClick(ClickContext click)
        {
            if (click.HitCollider == false)
                return;

            var targetRoot = click.HitCollider.GetComponentInParent<EntityRoot>();
            if (targetRoot == false)
                return;

            if (targetRoot.TryGetFeature<IInteractionTargetFeature>(out var shelfInteraction) == false)
                return;

            if (shelfInteraction.TryResolve(_player, out var interaction) == false)
                return;

            _pendingInteraction = interaction;
            _navigation.SetTarget(interaction.ApproachPoint);
        }

        private void CompletePendingInteraction()
        {
            if (_pendingInteraction == null)
                return;

            if (_pendingInteraction.CanExecute)
                _pendingInteraction.Execute();

            _pendingInteraction = null;
        }

        private void OnDestroy() => _disposables.Dispose();
    }
}
