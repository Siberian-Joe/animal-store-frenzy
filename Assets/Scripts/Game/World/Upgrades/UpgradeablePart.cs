using System;
using System.Collections.Generic;
using Game.World.Inventory;
using Game.World.Persistence;
using R3;
using UnityEngine;

namespace Game.World.Upgrades
{
    [DisallowMultipleComponent]
    public sealed class UpgradeablePart : StatefulEntityComponent<UpgradeableState>, IUpgradeable
    {
        [SerializeField] private UpgradeableDefinition _definition;
        [SerializeField] private UpgradeStageEntry[] _stages;

        private readonly Subject<Unit> _changed = new();

        public override int ActivationOrder => 280;
        public Observable<Unit> Changed => _changed;

        public UpgradeableDefinition Definition => _definition;
        public UpgradeStageId CurrentStageId => new(State.CurrentStageId);
        public bool HasNextStage => TryGetNextStage(out _);

        protected override StateSlotKey GetStateSlotKey() => StateSlotKey.For(typeof(UpgradeableState), "upgradeable");

        protected override void RestoreState(UpgradeableState state)
        {
            ValidateStages();
            if (string.IsNullOrWhiteSpace(state.CurrentStageId))
                state.CurrentStageId = _stages[0].StageId.Value;
        }

        protected override void InitializeFreshState(UpgradeableState state)
        {
            ValidateStages();
            state.CurrentStageId = _stages[0].StageId.Value;
        }

        protected override void ApplyBoundState(UpgradeableState state)
        {
            ValidateStages();
            if (TryGetCurrentStage(out var stage) == false)
            {
                throw new InvalidOperationException(
                    $"Upgradeable '{name}' has unknown current stage '{state.CurrentStageId}'.");
            }

            stage.ApplyVisuals();
        }

        public bool TryGetNextStage(out UpgradeStageEntry stage)
        {
            var index = GetCurrentStageIndex();
            if (index < 0 || index + 1 >= _stages.Length)
            {
                stage = null;
                return false;
            }

            stage = _stages[index + 1];
            return true;
        }

        public IReadOnlyList<ItemStack> GetNextStageCost()
        {
            return TryGetNextStage(out var stage)
                ? stage.BuildCost()
                : Array.Empty<ItemStack>();
        }

        public void Advance()
        {
            if (TryGetNextStage(out var nextStage) == false)
                throw new InvalidOperationException($"Upgradeable '{name}' has no next stage.");

            State.CurrentStageId = nextStage.StageId.Value;
            nextStage.ApplyVisuals();
            _changed.OnNext(Unit.Default);
        }

        public bool IsAtStage(UpgradeStageId stageId) => CurrentStageId == stageId;

        public bool MatchesDefinition(UpgradeableDefinition definition) =>
            definition != false && _definition == definition;

        protected override void OnDestroy()
        {
            _changed.Dispose();
            base.OnDestroy();
        }

        private bool TryGetCurrentStage(out UpgradeStageEntry stage)
        {
            var index = GetCurrentStageIndex();
            if (index < 0)
            {
                stage = null;
                return false;
            }

            stage = _stages[index];
            return true;
        }

        private int GetCurrentStageIndex()
        {
            if (string.IsNullOrWhiteSpace(State.CurrentStageId))
                return -1;

            var current = new UpgradeStageId(State.CurrentStageId);
            for (var index = 0; index < _stages.Length; index++)
            {
                if (_stages[index].StageId == current)
                    return index;
            }

            return -1;
        }

        private void ValidateStages()
        {
            if (_definition == false)
                throw new InvalidOperationException($"Upgradeable '{name}' requires an upgradeable definition.");

            _definition.Validate();

            if (_stages == null || _stages.Length == 0)
                throw new InvalidOperationException($"Upgradeable '{name}' requires at least one stage.");

            var seen = new HashSet<UpgradeStageId>();
            foreach (var stage in _stages)
            {
                if (stage == null)
                    throw new InvalidOperationException($"Upgradeable '{name}' contains a null stage entry.");

                stage.Validate(name);
                if (_definition.Contains(stage.StageId) == false)
                    throw new InvalidOperationException(
                        $"Upgradeable '{name}' contains stage '{stage.StageId}' not declared by its definition.");

                if (seen.Add(stage.StageId) == false)
                    throw new InvalidOperationException(
                        $"Upgradeable '{name}' contains duplicate stage '{stage.StageId}'.");
            }
        }
    }
}