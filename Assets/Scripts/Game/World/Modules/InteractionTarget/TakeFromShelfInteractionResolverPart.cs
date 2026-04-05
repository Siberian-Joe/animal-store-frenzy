using Game.World.Core;
using Game.World.Interactions;
using Game.World.ProductContainer;
using Game.World.ShelfConsumer;
using UnityEngine;

namespace Game.World.InteractionTarget
{
    public class TakeFromShelfInteractionResolverPart : InteractionResolverPart
    {
        public override bool TryResolve(
            EntityRoot initiator,
            EntityRoot target,
            IInteractionTargetFeature targetFeature,
            out IEntityInteraction interaction)
        {
            if (initiator == false ||
                target == false ||
                targetFeature == null ||
                initiator.TryGetFeature<IShelfConsumerFeature>(out var consumer) == false ||
                initiator.TryGetFeature<IProductContainerFeature>(out var receiver) == false ||
                target.TryGetFeature<IProductContainerFeature>(out var targetContainer) == false)
            {
                interaction = null;
                return false;
            }

            var amount = Mathf.Min(
                consumer.TransferAmount,
                targetContainer.Quantity.Value,
                receiver.FreeSpace);

            if (amount <= 0)
            {
                interaction = null;
                return false;
            }

            interaction = new TakeFromShelfInteraction(
                targetFeature.ApproachPoint,
                targetContainer,
                receiver,
                amount);

            return true;
        }
    }
}
