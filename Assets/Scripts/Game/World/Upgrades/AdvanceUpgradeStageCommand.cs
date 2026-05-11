using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Upgrades
{
    [Serializable]
    public sealed class AdvanceUpgradeStageCommand : IGameCommand
    {
        public AdvanceUpgradeStageCommand(EntityId actorId, EntityId upgradeableId)
        {
            ActorId = actorId;
            UpgradeableId = upgradeableId;
        }

        public EntityId ActorId { get; }
        public EntityId UpgradeableId { get; }
    }
}