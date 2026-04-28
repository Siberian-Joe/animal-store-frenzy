using System.Collections.Generic;
using UnityEngine;

namespace Game.World.Interactions
{
    public interface IInteractionOptionProvider
    {
        int Order { get; }

        void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options);
    }
}