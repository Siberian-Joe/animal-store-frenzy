using Game.World.Core;
using Game.World.Interactions;
using Game.World.ProductContainer;
using Game.World.ShelfRestocker;
using UnityEngine;

namespace Game.World.InteractionTarget
{
    public class RestockShelfInteractionResolverPart : InteractionResolverPart
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
                initiator.TryGetFeature<IShelfRestockerFeature>(out var restocker) == false ||
                initiator.TryGetFeature<IProductContainerFeature>(out var source) == false ||
                target.TryGetFeature<IProductContainerFeature>(out var targetContainer) == false)
            {
                interaction = null;
                return false;
            }

            var amount = Mathf.Min(
                restocker.TransferAmount,
                source.Quantity.Value,
                targetContainer.FreeSpace);

            if (amount <= 0)
            {
                interaction = null;
                return false;
            }

            interaction = new RestockShelfInteraction(
                targetFeature.ApproachPoint,
                source,
                targetContainer,
                amount);

            return true;
        }
    }
}
