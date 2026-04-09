using System;
using Game.World.Interactions;
using Game.World.Interactions.Shelf;
using Game.World.Features.ProductContainer;
using UnityEngine;

namespace Game.World.Features.InteractionTarget
{
    [RequireComponent(typeof(ProductContainerPart))]
    public class TakeFromShelfCommandBuilderPart : InteractionCommandBuilderPart
    {
        [SerializeField] private ProductContainerPart _container;

        protected virtual void Awake() => EnsureDependencies();

        protected virtual void OnValidate() => AutoAssignDependencies();

        public override bool TryBuild(
            IInteractionRoleResolver source,
            Vector3 approachPoint,
            out InteractionCommandRequest request)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.TryResolve<ITakeReceiverContract>(out var receiverContract) == false)
            {
                request = null;
                return false;
            }

            var sourceContract = Container;
            var amount = Mathf.Min(
                receiverContract.TransferAmount,
                sourceContract.AvailableQuantity,
                receiverContract.FreeSpace);

            if (amount <= 0)
            {
                request = null;
                return false;
            }

            request = new InteractionCommandRequest(
                approachPoint,
                new TakeFromShelfCommand(sourceContract, receiverContract, amount));

            return true;
        }

        protected ProductContainerPart Container =>
            _container != null
                ? _container
                : throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' requires {nameof(ProductContainerPart)}.");

        private void AutoAssignDependencies()
        {
            if (_container == null)
                TryGetComponent(out _container);
        }

        private void EnsureDependencies()
        {
            AutoAssignDependencies();
            _ = Container;
        }
    }
}
