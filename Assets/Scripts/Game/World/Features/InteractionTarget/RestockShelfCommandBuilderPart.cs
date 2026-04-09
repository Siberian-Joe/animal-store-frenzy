using System;
using Game.World.Interactions;
using Game.World.Interactions.Shelf;
using Game.World.Features.ProductContainer;
using UnityEngine;

namespace Game.World.Features.InteractionTarget
{
    [RequireComponent(typeof(ProductContainerPart))]
    public class RestockShelfCommandBuilderPart : InteractionCommandBuilderPart
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

            if (source.TryResolve<IRestockSourceContract>(out var sourceContract) == false)
            {
                request = null;
                return false;
            }

            var targetContract = Container;
            var amount = Mathf.Min(
                sourceContract.TransferAmount,
                sourceContract.AvailableQuantity,
                targetContract.FreeSpace);

            if (amount <= 0)
            {
                request = null;
                return false;
            }

            request = new InteractionCommandRequest(
                approachPoint,
                new RestockShelfCommand(sourceContract, targetContract, amount));

            return true;
        }

        protected ProductContainerPart Container =>
            _container
                ? _container
                : throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' requires {nameof(ProductContainerPart)}.");

        private void AutoAssignDependencies()
        {
            if (_container == false)
                TryGetComponent(out _container);
        }

        private void EnsureDependencies()
        {
            AutoAssignDependencies();
            _ = Container;
        }
    }
}