using System;
using Game.World.Persistence;

namespace Game.World.Upgrades
{
    [Serializable]
    public sealed class UpgradeableState : IEntityStateData
    {
        public string CurrentStageId;

        public IEntityStateData DeepClone() => new UpgradeableState
        {
            CurrentStageId = CurrentStageId
        };
    }
}