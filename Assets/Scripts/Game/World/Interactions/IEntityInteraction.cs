using UnityEngine;

namespace Game.World.Interactions
{
    public interface IEntityInteraction
    {
        Vector3 ApproachPoint { get; }

        bool CanExecute { get; }

        void Execute();
    }
}
