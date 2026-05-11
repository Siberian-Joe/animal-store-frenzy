using System;
using System.Collections.Generic;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Upgrades
{
    [Serializable]
    public sealed class UpgradeStageEntry
    {
        [SerializeField] private string _stageId;
        [SerializeField] private ItemStackDefinition[] _costFromPreviousStage;
        [SerializeField] private GameObject[] _activeVisualObjects;
        [SerializeField] private GameObject[] _inactiveVisualObjects;

        public UpgradeStageId StageId => new(_stageId);

        public IReadOnlyList<ItemStackDefinition> CostFromPreviousStage =>
            _costFromPreviousStage ?? Array.Empty<ItemStackDefinition>();

        public IReadOnlyList<ItemStack> BuildCost()
        {
            var result = new List<ItemStack>();
            if (_costFromPreviousStage == null)
                return result;

            foreach (var cost in _costFromPreviousStage)
            {
                if (cost == null)
                    continue;

                result.Add(cost.ToStack());
            }

            return result;
        }

        public void ApplyVisuals()
        {
            SetActive(_activeVisualObjects, true);
            SetActive(_inactiveVisualObjects, false);
        }

        public void Validate(string ownerName)
        {
            if (string.IsNullOrWhiteSpace(_stageId))
                throw new InvalidOperationException($"Upgradeable '{ownerName}' contains a stage with an empty id.");
        }

        private static void SetActive(GameObject[] objects, bool isActive)
        {
            if (objects == null)
                return;

            foreach (var target in objects)
            {
                if (target)
                    target.SetActive(isActive);
            }
        }
    }
}