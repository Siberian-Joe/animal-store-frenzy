using Game.World.Core;

namespace Game.World.Interactions
{
    public interface IInteractionResolver
    {
        int Order { get; }

        bool TryResolve(
            EntityRoot initiator,
            EntityRoot target,
            IInteractionTargetFeature targetFeature,
            out IEntityInteraction interaction);
    }
}
