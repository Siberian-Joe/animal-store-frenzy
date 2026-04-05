using UnityEngine;

using Game.World.Interactions;
using Game.World.ProductContainer;

namespace Game.World.InteractionTarget
{
    public sealed class TakeFromShelfInteraction : IEntityInteraction
    {
        private readonly IProductContainerFeature _shelf;
        private readonly IProductContainerFeature _receiver;
        private readonly int _amount;

        public TakeFromShelfInteraction(
            Vector3 approachPoint,
            IProductContainerFeature shelf,
            IProductContainerFeature receiver,
            int amount)
        {
            ApproachPoint = approachPoint;
            _shelf = shelf;
            _receiver = receiver;
            _amount = amount;
        }

        public Vector3 ApproachPoint { get; }

        public bool CanExecute =>
            _amount > 0 &&
            _shelf.IsEmpty == false &&
            _receiver.IsFull == false;

        public void Execute()
        {
            if (CanExecute == false)
                return;

            var removed = _shelf.RemoveUpTo(_amount);
            if (removed <= 0)
                return;

            var accepted = _receiver.AddUpTo(removed);
            var remainder = removed - accepted;

            if (remainder > 0)
                _shelf.AddUpTo(remainder);
        }
    }
}
