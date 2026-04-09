using UnityEngine;

namespace Game.World.Interactions
{
    public abstract class InteractionCommandBuilderPart : MonoBehaviour, IInteractionCommandBuilder
    {
        [SerializeField] private int _order;

        public int Order => _order;

        public abstract bool TryBuild(
            IInteractionRoleResolver source,
            Vector3 approachPoint,
            out InteractionCommandRequest request);
    }
}
