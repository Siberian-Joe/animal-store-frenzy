using System;
using Game.World.EntityRuntime;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Checkouts
{
    [DisallowMultipleComponent]
    public sealed class CheckoutCounterPart : EntityComponent, ICheckoutServicePoint
    {
        [Inject] private readonly IShopInteractionRegistry _shopInteractionRegistry;

        public override int ActivationOrder => 250;

        protected override void OnActivate()
        {
            if (_shopInteractionRegistry == null)
                throw new InvalidOperationException(
                    $"{nameof(CheckoutCounterPart)} on '{name}' requires {nameof(IShopInteractionRegistry)} injection.");

            _shopInteractionRegistry.Register(this);
        }

        protected override void OnDeactivate()
        {
            _shopInteractionRegistry?.Unregister(this);
        }
    }
}