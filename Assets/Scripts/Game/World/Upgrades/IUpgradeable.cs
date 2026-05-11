using System.Collections.Generic;
using Game.World.Inventory;
using R3;

namespace Game.World.Upgrades
{
    public interface IUpgradeable
    {
        UpgradeStageId CurrentStageId { get; }
        Observable<Unit> Changed { get; }
        bool HasNextStage { get; }
        bool TryGetNextStage(out UpgradeStageEntry stage);
        IReadOnlyList<ItemStack> GetNextStageCost();
        void Advance();
        bool IsAtStage(UpgradeStageId stageId);
    }
}