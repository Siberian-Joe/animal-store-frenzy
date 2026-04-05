using Game.World.Core;
using UnityEngine;

namespace Game.World.Interactions
{
    public abstract class InteractionResolverPart : MonoBehaviour, IInteractionResolver
    {
        [SerializeField] private int _order;

        public int Order => _order;

        public abstract bool TryResolve(
            EntityRoot initiator,
            EntityRoot target,
            IInteractionTargetFeature targetFeature,
            out IEntityInteraction interaction);
    }
}
