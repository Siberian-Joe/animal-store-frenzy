using System.Collections.Generic;
using Game.World.Commands;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Features.Navigation;
using Game.World.Interactions;
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
        private readonly List<InteractionOption> _interactionOptions = new(4);

        private IPlayerInputService _inputService;
        private GameCommandDispatcher _commandDispatcher;
        private InteractionOption _pendingOption;
        private InteractionActorPart _sourcePart;
        private INavigationFeature _navigation;

        [Inject]
        public void Construct(
            IPlayerInputService inputService,
            GameCommandDispatcher commandDispatcher)
        {
            _inputService = inputService;
            _commandDispatcher = commandDispatcher;
        }

        private void Start()
        {
            if (_player == false)
            {
                Debug.LogError($"{nameof(PlayerShelfInteractionTester)}: player entity is not assigned", this);
                return;
            }

            if (_commandDispatcher == null)
            {
                Debug.LogError($"{nameof(PlayerShelfInteractionTester)}: command dispatcher was not injected", this);
                return;
            }

            if (_inputService == null)
            {
                Debug.LogError($"{nameof(PlayerShelfInteractionTester)}: input service was not injected", this);
                return;
            }

            if (TryGetNavigation(_player, out _navigation) == false)
            {
                Debug.LogError($"{nameof(PlayerShelfInteractionTester)}: player has no navigation feature", this);
                return;
            }

            if (TryGetInteractionSource(_player, out _sourcePart) == false)
            {
                Debug.LogError(
                    $"{nameof(PlayerShelfInteractionTester)}: player has no {nameof(InteractionActorPart)}",
                    this);
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

            if (TryGetInteractionTarget(targetRoot, out var targetPoint) == false)
                return;

            _interactionOptions.Clear();
            targetPoint.CollectOptions(_sourcePart, _interactionOptions);

            if (_interactionOptions.Count <= 0)
                return;

            _pendingOption = _interactionOptions[0];
            _navigation.SetTarget(_pendingOption.ApproachPoint);
        }

        private void CompletePendingInteraction()
        {
            if (_pendingOption == null)
                return;

            _commandDispatcher.Dispatch(_pendingOption.Command);
            _pendingOption = null;
        }

        private static bool TryGetNavigation(EntityRoot root, out INavigationFeature navigation)
        {
            if (root == false)
            {
                navigation = null;
                return false;
            }

            var candidates = root.GetComponentsInChildren<NavigationPart>(true);

            foreach (var candidate in candidates)
            {
                if (candidate == false)
                    continue;

                if (candidate.GetComponentInParent<EntityRoot>() != root)
                    continue;

                navigation = candidate;
                return true;
            }

            navigation = null;
            return false;
        }

        private static bool TryGetInteractionTarget(EntityRoot root, out InteractionTargetPart targetPoint)
        {
            if (root == false)
            {
                targetPoint = null;
                return false;
            }

            var candidates = root.GetComponentsInChildren<InteractionTargetPart>(true);

            foreach (var candidate in candidates)
            {
                if (candidate == false)
                    continue;

                if (candidate.GetComponentInParent<EntityRoot>() != root)
                    continue;

                targetPoint = candidate;
                return true;
            }

            targetPoint = null;
            return false;
        }

        private static bool TryGetInteractionSource(EntityRoot root, out InteractionActorPart sourcePoint)
        {
            if (root == false)
            {
                sourcePoint = null;
                return false;
            }

            var candidates = root.GetComponentsInChildren<InteractionActorPart>(true);

            foreach (var candidate in candidates)
            {
                if (candidate == false)
                    continue;

                if (candidate.GetComponentInParent<EntityRoot>() != root)
                    continue;

                sourcePoint = candidate;
                return true;
            }

            sourcePoint = null;
            return false;
        }

        private void OnDestroy() => _disposables.Dispose();
    }
}