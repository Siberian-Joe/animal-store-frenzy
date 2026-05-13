using System;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    [DisallowMultipleComponent]
    public sealed class CustomerSpawnInitializerPart :
        EntityComponent,
        ICustomerSpawnInitializer
    {
        [Header("References")] [SerializeField]
        private CustomerProfilePart _customerProfile;

        [SerializeField] private CustomerNeedsPart _customerNeeds;
        [SerializeField] private CustomerBasketPart _customerBasket;
        [SerializeField] private CustomerCheckoutProgressPart _checkoutProgress;

        public override int ActivationOrder => 340;

        protected override void OnActivate()
        {
            _customerProfile ??= OwnerRoot.FindOwnedComponent<CustomerProfilePart>();
            _customerNeeds ??= OwnerRoot.FindOwnedComponent<CustomerNeedsPart>();
            _customerBasket ??= OwnerRoot.FindOwnedComponent<CustomerBasketPart>();
            _checkoutProgress ??= OwnerRoot.FindOwnedComponent<CustomerCheckoutProgressPart>();

            if (_customerProfile == false)
                throw new InvalidOperationException(
                    $"{nameof(CustomerSpawnInitializerPart)} on '{name}' requires {nameof(CustomerProfilePart)} under the same EntityRoot.");

            if (_customerNeeds == false)
                throw new InvalidOperationException(
                    $"{nameof(CustomerSpawnInitializerPart)} on '{name}' requires {nameof(CustomerNeedsPart)} under the same EntityRoot.");

            if (_customerBasket == false)
                throw new InvalidOperationException(
                    $"{nameof(CustomerSpawnInitializerPart)} on '{name}' requires {nameof(CustomerBasketPart)} under the same EntityRoot.");

            if (_checkoutProgress == false)
                throw new InvalidOperationException(
                    $"{nameof(CustomerSpawnInitializerPart)} on '{name}' requires {nameof(CustomerCheckoutProgressPart)} under the same EntityRoot.");
        }

        public void ApplySpawnRequest(CustomerSpawnRequest request)
        {
            if (IsActive == false)
                throw new InvalidOperationException(
                    $"{nameof(CustomerSpawnInitializerPart)} on '{name}' cannot apply spawn request before activation.");

            _customerProfile.SetArchetype(request.ArchetypeId);
            _customerBasket.ClearBasket();
            _checkoutProgress.ResetCheckoutProgress();
            _customerNeeds.ReplaceNeeds(request.InitialNeeds);
        }
    }
}