using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.UtilityAi
{
    [DisallowMultipleComponent]
    public sealed class UtilityBrainPart : EntityComponent
    {
        [SerializeField] private UtilityDecisionAuthoringPart _authoring;
        [SerializeField, Min(0.01f)] private float _thinkInterval = 0.25f;

        private readonly UtilityFactRuntime _facts = new();
        private readonly List<IUtilityOption> _optionsBuffer = new(16);

        private RunningDecision _currentDecision;
        private float _timeUntilNextThink;

        public override int ActivationOrder => 600;

        protected override void OnActivate()
        {
            _authoring ??= OwnerRoot.FindOwnedComponent<UtilityDecisionAuthoringPart>();

            if (_authoring == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(UtilityBrainPart)} on '{name}' requires {nameof(UtilityDecisionAuthoringPart)} under the same EntityRoot.");
            }

            BindConsiderations();
            _timeUntilNextThink = 0f;
        }

        protected override void OnDeactivate()
        {
            try
            {
                CancelCurrentDecision();
            }
            finally
            {
                _optionsBuffer.Clear();
                _facts.Clear();
            }
        }

        private void Update()
        {
            if (IsActive == false)
                return;

            TickCurrentAction(Time.deltaTime);

            _timeUntilNextThink -= Time.deltaTime;
            if (_timeUntilNextThink > 0f)
                return;

            if (TryGetCurrentAction(out var currentAction) && currentAction.IsInterruptible == false)
                return;

            Think();
            _timeUntilNextThink = _thinkInterval;
        }

        private void BindConsiderations()
        {
            var bindings = _authoring.Bindings;

            foreach (var binding in bindings)
            {
                var considerations = binding.Considerations;

                foreach (var consideration in considerations)
                {
                    if (consideration == false)
                        continue;

                    consideration.Bind(_facts);
                }
            }
        }

        private void TickCurrentAction(float deltaTime)
        {
            if (TryGetCurrentAction(out var action) == false)
                return;

            var status = action.TickExecution(deltaTime);
            if (status == UtilityActionStatus.Running)
                return;

            CancelCurrentDecision();
        }

        private void Think()
        {
            UtilityActionNodePart bestAction = null;
            IUtilityOption bestOption = null;
            var bestScore = float.MinValue;

            var bindings = _authoring.Bindings;

            foreach (var binding in bindings)
            {
                if (binding.Action == false)
                    continue;

                _optionsBuffer.Clear();
                binding.Action.CollectOptions(_optionsBuffer);

                foreach (var option in _optionsBuffer)
                {
                    if (option == null)
                        continue;

                    _facts.Clear();
                    option.WriteFacts(_facts);

                    var score = CalculateCompensatedScore(
                        binding.Action.BaseScore,
                        binding.Considerations);

                    if (score <= 0f)
                        continue;

                    if (_currentDecision != null &&
                        _currentDecision.Action != false &&
                        ReferenceEquals(_currentDecision.Action, binding.Action) &&
                        _currentDecision.Option != null &&
                        _currentDecision.Option.IsEquivalentTo(option))
                    {
                        score = Mathf.Clamp01(score + binding.Action.InertiaBonus);
                    }

                    if (IsBetterCandidate(
                            binding.Action,
                            score,
                            bestAction,
                            bestScore) == false)
                    {
                        continue;
                    }

                    bestScore = score;
                    bestAction = binding.Action;
                    bestOption = option;
                }
            }

            if (bestAction == false || bestOption == null)
                return;

            if (_currentDecision != null &&
                _currentDecision.Action != false &&
                ReferenceEquals(_currentDecision.Action, bestAction) &&
                _currentDecision.Option != null &&
                _currentDecision.Option.IsEquivalentTo(bestOption))
            {
                return;
            }

            CancelCurrentDecision();

            _currentDecision = new RunningDecision(bestAction, bestOption);
            _currentDecision.Action.BeginExecution(_currentDecision.Option);
        }

        private static float CalculateCompensatedScore(
            float baseScore,
            IReadOnlyList<UtilityConsiderationPart> considerations)
        {
            var score = Mathf.Clamp01(baseScore);

            if (considerations == null || considerations.Count == 0)
                return score;

            var considerationCount = 0;

            foreach (var consideration in considerations)
            {
                if (consideration != false)
                    considerationCount++;
            }

            if (considerationCount <= 0)
                return score;

            var modificationFactor = 1f - (1f / considerationCount);

            foreach (var consideration in considerations)
            {
                if (consideration == false)
                    continue;

                var rawScore = Mathf.Clamp01(consideration.Evaluate());
                if (rawScore <= 0f)
                    return 0f;

                var makeUpValue = (1f - rawScore) * modificationFactor;
                var compensatedScore = rawScore + (makeUpValue * rawScore);

                score *= Mathf.Clamp01(compensatedScore);

                if (score <= 0f)
                    return 0f;
            }

            return Mathf.Clamp01(score);
        }

        private bool TryGetCurrentAction(out UtilityActionNodePart action)
        {
            action = null;

            if (_currentDecision == null)
                return false;

            action = _currentDecision.Action;
            if (action != false)
                return true;

            _currentDecision = null;
            return false;
        }

        private void CancelCurrentDecision()
        {
            if (_currentDecision == null)
                return;

            var action = _currentDecision.Action;
            _currentDecision = null;

            if (action != false)
                action.CancelExecution();
        }

        private static bool IsBetterCandidate(
            UtilityActionNodePart candidateAction,
            float candidateScore,
            UtilityActionNodePart currentBestAction,
            float currentBestScore)
        {
            if (currentBestAction == false)
                return true;

            if (candidateScore > currentBestScore)
                return true;

            if (Mathf.Approximately(candidateScore, currentBestScore) == false)
                return false;

            return candidateAction.SelectionOrder < currentBestAction.SelectionOrder;
        }

        private sealed class RunningDecision
        {
            public RunningDecision(UtilityActionNodePart action, IUtilityOption option)
            {
                if (action == false)
                    throw new ArgumentNullException(nameof(action));

                Action = action;
                Option = option ?? throw new ArgumentNullException(nameof(option));
            }

            public UtilityActionNodePart Action { get; }

            public IUtilityOption Option { get; }
        }
    }
}