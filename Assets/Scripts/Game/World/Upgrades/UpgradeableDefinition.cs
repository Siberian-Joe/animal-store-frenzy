using System;
using System.Collections.Generic;
using Game.World.Authoring;
using UnityEngine;

namespace Game.World.Upgrades
{
    [CreateAssetMenu(
        fileName = "UpgradeableDefinition",
        menuName = "Game/World/Upgrades/Upgradeable Definition")]
    public sealed class UpgradeableDefinition : StableIdDefinition
    {
        [SerializeField] private string[] _stageIds;

        public bool Contains(UpgradeStageId stageId)
        {
            Validate();

            foreach (var stage in _stageIds)
            {
                if (new UpgradeStageId(stage) == stageId)
                    return true;
            }

            return false;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_stageIds == null)
                return;

            for (var index = 0; index < _stageIds.Length; index++)
                _stageIds[index] = _stageIds[index]?.Trim();
        }

        public void Validate()
        {
            ValidateStableId();

            if (_stageIds == null || _stageIds.Length == 0)
                throw new InvalidOperationException(
                    $"{nameof(UpgradeableDefinition)} '{name}' requires at least one stage id.");

            var seen = new HashSet<UpgradeStageId>();
            foreach (var stageId in _stageIds)
            {
                if (string.IsNullOrWhiteSpace(stageId))
                    throw new InvalidOperationException(
                        $"{nameof(UpgradeableDefinition)} '{name}' contains an empty stage id.");

                var typedId = new UpgradeStageId(stageId);
                if (seen.Add(typedId) == false)
                    throw new InvalidOperationException(
                        $"{nameof(UpgradeableDefinition)} '{name}' contains duplicate stage '{typedId}'.");
            }
        }
    }
}