using Game.World.EntityRuntime;
using Game.World.Features.Navigation;
using R3;
using UnityEngine;

namespace Game.World.Debugging
{
    public sealed class ClickToMoveTester : MonoBehaviour
    {
        [SerializeField] private EntityRoot _targetEntity;

        private readonly CompositeDisposable _disposables = new();

        // private IPlayerInputService _playerInputService;

        // [Inject]
        // public void Construct(IPlayerInputService playerInputService)
        // {
        //     _playerInputService = playerInputService;
        // }

        private void Start()
        {
            // if (_playerInputService == null)
            // {
            //     Debug.LogError($"{nameof(ClickToMoveTester)}: {nameof(IPlayerInputService)} was not injected.",
            //         this);
            //     return;
            // }

            if (_targetEntity == false)
            {
                Debug.LogError($"{nameof(ClickToMoveTester)}: target entity is not assigned.", this);
                return;
            }

            // _playerInputService.Clicked
            //     .Subscribe(HandleClick)
            //     .AddTo(_disposables);
        }

        // private void HandleClick(ClickContext click)
        // {
        //     if (TryGetNavigation(_targetEntity, out var navigation) == false)
        //         return;
        //
        //     navigation.SetTarget(click.WorldPosition);
        // }

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

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}