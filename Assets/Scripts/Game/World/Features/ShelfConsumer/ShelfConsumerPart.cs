using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Features.ProductContainer;
using UnityEngine;

namespace Game.World.Features.ShelfConsumer
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InteractionSourcePart))]
    [RequireComponent(typeof(ProductContainerPart))]
    public sealed class ShelfConsumerPart : EntityComponent,
        IShelfConsumerFeature,
        ITakeReceiverContract,
        IInteractionSourceRoleOwner
    {
        [SerializeField]
        [Min(1)]
        private int _transferAmount = 1;

        [SerializeField]
        private ProductContainerPart _container;

        public override int ActivationOrder => 350;

        public int TransferAmount => Mathf.Max(1, _transferAmount);

        public int FreeSpace => Container.FreeSpace;

        public bool IsFull => Container.IsFull;

        private void Awake() => EnsureDependencies();

        private void OnValidate() => AutoAssignDependencies();

        public int AddUpTo(int amount) => Container.AddUpTo(amount);

        private ProductContainerPart Container =>
            _container != null
                ? _container
                : throw new System.InvalidOperationException(
                    $"{nameof(ShelfConsumerPart)} on '{name}' requires {nameof(ProductContainerPart)}.");

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
