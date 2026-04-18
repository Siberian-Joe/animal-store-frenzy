using System;
using Game.World.Commands;
using Game.World.Features.Navigation;
using Game.World.Interactions;
using UnityEngine;
using Zenject;

namespace Game.World.UtilityAi
{
    public abstract class CustomerInteractionActionPart<TOption> : UtilityActionNodePart
        where TOption : CustomerShopOption
    {
        [Header("Execution")] [SerializeField] private NavigationPart _navigation;
        [SerializeField] private InteractionActorPart _interactionActor;
        [SerializeField, Min(0f)] private float _arrivalDistanceEpsilon = 0.05f;

        private GameCommandDispatcher _commandDispatcher;
        private IUtilityReachabilityEvaluator _reachabilityEvaluator;
        private TOption _runningOption;

        [Inject]
        public void ConstructExecution(
            GameCommandDispatcher commandDispatcher,
            IUtilityReachabilityEvaluator reachabilityEvaluator)
        {
            _commandDispatcher = commandDispatcher;
            _reachabilityEvaluator = reachabilityEvaluator;
        }

        protected override void OnActionActivated()
        {
            _navigation ??= OwnerRoot.FindOwnedComponent<NavigationPart>();
            _interactionActor ??= OwnerRoot.FindOwnedComponent<InteractionActorPart>();

            if (_navigation == false)
                throw new InvalidOperationException($"{GetType().Name} requires {nameof(NavigationPart)}.");

            if (_interactionActor == false)
                throw new InvalidOperationException($"{GetType().Name} requires {nameof(InteractionActorPart)}.");

            if (_commandDispatcher == null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} requires {nameof(GameCommandDispatcher)} injection.");
            }

            if (_reachabilityEvaluator == null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} requires {nameof(IUtilityReachabilityEvaluator)} injection.");
            }
        }

        public override void BeginExecution(IUtilityOption option)
        {
            if (option is not TOption typedOption)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} cannot execute option '{option?.GetType().Name ?? "<null>"}'.");
            }

            if (TryResolveInteraction(typedOption, out _) == false)
                return;

            _runningOption = typedOption;
            _navigation.SetTarget(typedOption.ApproachPoint);
        }

        public override UtilityActionStatus TickExecution(float deltaTime)
        {
            if (_runningOption == null)
                return UtilityActionStatus.Failed;

            if (TryResolveInteraction(_runningOption, out var interaction) == false)
            {
                CancelExecution();
                return UtilityActionStatus.Failed;
            }

            if (IsAtApproachPoint(_runningOption.ApproachPoint) == false)
                return UtilityActionStatus.Running;

            _commandDispatcher.Dispatch(interaction.Command);
            CancelExecution();
            return UtilityActionStatus.Completed;
        }

        public override void CancelExecution()
        {
            _runningOption = null;
            _navigation?.ClearTarget();
        }

        protected UtilityReachabilityEvaluation EvaluateTarget(
            Vector3 targetPosition,
            out Vector3 navigationTarget)
        {
            var origin = OwnerRoot ? OwnerRoot.transform.position : transform.position;

            if (_reachabilityEvaluator.TryEvaluate(origin, targetPosition, out var evaluation, out navigationTarget))
                return evaluation;

            navigationTarget = targetPosition;
            return UtilityReachabilityEvaluation.Unreachable(
                Vector3.Distance(origin, targetPosition));
        }

        protected bool TryResolveInteraction(TOption option, out InteractionOption interaction)
        {
            interaction = null;

            if (option == null || option.TargetRoot == false)
                return false;

            if (option.InteractionTarget == false || option.InteractionTarget.IsActive == false)
                return false;

            if (option.InteractionTarget.TryGetPrimaryOption(_interactionActor, out var primary) == false)
                return false;

            if (option.MatchesInteraction(primary) == false)
                return false;

            interaction = primary;
            return true;
        }

        private bool IsAtApproachPoint(Vector3 approachPoint)
        {
            var currentPosition = OwnerRoot ? OwnerRoot.transform.position : transform.position;
            var maxDistance = Mathf.Max(0f, _navigation.StoppingDistance) + _arrivalDistanceEpsilon;

            return (currentPosition - approachPoint).sqrMagnitude <= maxDistance * maxDistance;
        }
    }
}