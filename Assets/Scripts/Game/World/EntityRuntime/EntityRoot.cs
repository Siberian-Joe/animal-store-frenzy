using UnityEngine;

namespace Game.World.EntityRuntime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentifier))]
    public sealed class EntityRoot : MonoBehaviour
    {
        private EntityIdentifier _identifier;

        public EntityIdentifier Identifier
        {
            get
            {
                _identifier ??= GetComponent<EntityIdentifier>();
                return _identifier;
            }
        }

        public EntityId Id => new(Identifier.Id);
    }
}
