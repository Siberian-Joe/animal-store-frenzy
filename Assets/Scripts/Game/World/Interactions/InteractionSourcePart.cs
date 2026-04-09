using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Interactions
{
    [DisallowMultipleComponent]
    public sealed class InteractionSourcePart : EntityComponent, IInteractionRoleResolver
    {
        private Dictionary<Type, IInteractionContract> _contractsByType;

        public override int ActivationOrder => 360;

        protected override void OnActivate() => EnsureCached();

        public bool TryResolve<TContract>(out TContract contract)
            where TContract : class, IInteractionContract
        {
            EnsureCached();

            if (_contractsByType.TryGetValue(typeof(TContract), out var registeredContract) &&
                registeredContract is TContract typedContract)
            {
                contract = typedContract;
                return true;
            }

            contract = null;
            return false;
        }

        private void EnsureCached()
        {
            if (_contractsByType != null)
                return;

            if (OwnerRoot == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(InteractionSourcePart)} on '{name}' must live under an {nameof(EntityRoot)}.");
            }

            var contractsByType = new Dictionary<Type, IInteractionContract>();
            var behaviours = OwnerRoot.GetComponentsInChildren<MonoBehaviour>(true);

            foreach (var behaviour in behaviours)
            {
                if (behaviour == false || BelongsToRoot(behaviour) == false)
                    continue;

                if (behaviour is not IInteractionSourceRoleOwner || behaviour is not IInteractionContract contractOwner)
                    continue;

                RegisterOwnedContracts(behaviour, contractOwner, contractsByType);
            }

            _contractsByType = contractsByType;
        }

        private void RegisterOwnedContracts(
            MonoBehaviour ownerBehaviour,
            IInteractionContract contractOwner,
            IDictionary<Type, IInteractionContract> contractsByType)
        {
            var ownerType = ownerBehaviour.GetType();
            var hasSpecificContract = false;

            foreach (var interfaceType in ownerType.GetInterfaces())
            {
                if (typeof(IInteractionContract).IsAssignableFrom(interfaceType) == false ||
                    interfaceType == typeof(IInteractionContract))
                {
                    continue;
                }

                hasSpecificContract = true;

                if (contractsByType.TryGetValue(interfaceType, out var existingContract))
                {
                    throw new InvalidOperationException(
                        $"Duplicate source interaction role '{interfaceType.Name}' detected under entity " +
                        $"'{OwnerRoot.name}'. Existing owner: '{existingContract.GetType().Name}', " +
                        $"new owner: '{ownerType.Name}'.");
                }

                contractsByType.Add(interfaceType, contractOwner);
            }

            if (hasSpecificContract == false)
            {
                throw new InvalidOperationException(
                    $"Source interaction role owner '{ownerType.Name}' on '{name}' does not expose a specific " +
                    $"{nameof(IInteractionContract)}.");
            }
        }

        private bool BelongsToRoot(Component component) => component.GetComponentInParent<EntityRoot>() == OwnerRoot;
    }
}
