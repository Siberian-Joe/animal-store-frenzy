using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Processing
{
    [Serializable]
    public sealed class ProcessItemsCommand : IGameCommand
    {
        public EntityId ActorId { get; }
        public EntityId ProcessingPointId { get; }

        public ProcessItemsCommand(EntityId actorId, EntityId processingPointId)
        {
            ActorId = actorId;
            ProcessingPointId = processingPointId;
        }
    }
}
