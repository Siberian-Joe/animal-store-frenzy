using System;
using UnityEngine;

namespace Game.World.Upgrades
{
    [Serializable]
    public sealed class UpgradeStageReference
    {
        [SerializeField] private UpgradeableDefinition _definition;
        [SerializeField] private string _stageId;

        public UpgradeableDefinition Definition => _definition;
        public UpgradeStageId StageId => new(_stageId);
        public bool IsEmpty => _definition == false && string.IsNullOrWhiteSpace(_stageId);

        public void Validate(string ownerName = null)
        {
            var context = string.IsNullOrWhiteSpace(ownerName)
                ? nameof(UpgradeStageReference)
                : ownerName;

            if (_definition == false)
                throw new InvalidOperationException($"{context} requires an upgradeable definition.");

            if (string.IsNullOrWhiteSpace(_stageId))
                throw new InvalidOperationException($"{context} requires a stage id.");

            _definition.Validate();

            if (_definition.Contains(StageId) == false)
                throw new InvalidOperationException($"{context} references unknown upgrade stage '{StageId}'.");
        }
    }
}