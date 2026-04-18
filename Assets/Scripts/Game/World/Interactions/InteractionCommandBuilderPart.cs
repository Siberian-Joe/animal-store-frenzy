using System.Collections.Generic;
using UnityEngine;

namespace Game.World.Interactions
{
    public abstract class InteractionCommandBuilderPart : MonoBehaviour, IInteractionOptionProvider
    {
        [SerializeField] private int _order;

        public int Order => _order;

        public abstract void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options);
    }
}
