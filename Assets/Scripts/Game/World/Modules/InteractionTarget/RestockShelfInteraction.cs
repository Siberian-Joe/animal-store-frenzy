using UnityEngine;

using Game.World.Interactions;
using Game.World.ProductContainer;

namespace Game.World.InteractionTarget
{
    public sealed class RestockShelfInteraction : IEntityInteraction
    {
        private readonly IProductContainerFeature _source;
        private readonly IProductContainerFeature _shelf;
        private readonly int _amount;

        public RestockShelfInteraction(
            Vector3 approachPoint,
            IProductContainerFeature source,
            IProductContainerFeature shelf,
            int amount)
        {
            ApproachPoint = approachPoint;
            _source = source;
            _shelf = shelf;
            _amount = amount;
        }

        public Vector3 ApproachPoint { get; }

        public bool CanExecute =>
            _amount > 0 &&
            _source.IsEmpty == false &&
            _shelf.IsFull == false;

        public void Execute()
        {
            if (CanExecute == false)
                return;

            var removed = _source.RemoveUpTo(_amount);
            if (removed <= 0)
                return;

            var accepted = _shelf.AddUpTo(removed);
            var remainder = removed - accepted;

            if (remainder > 0)
                _source.AddUpTo(remainder);
        }
    }
}
