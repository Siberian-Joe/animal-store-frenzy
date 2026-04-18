using System;
using Game.World.EntityRuntime;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Exits
{
    [DisallowMultipleComponent]
    public sealed class StoreExitPointPart : EntityComponent, IStoreExitPoint
    {
        private IShopInteractionRegistry _shopInteractionRegistry;

        public override int ActivationOrder => 260;

        [Inject]
        public void Construct(IShopInteractionRegistry shopInteractionRegistry) =>
            _shopInteractionRegistry = shopInteractionRegistry;

        protected override void OnActivate()
        {
            if (_shopInteractionRegistry == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(StoreExitPointPart)} on '{name}' requires {nameof(IShopInteractionRegistry)} injection.");
            }

            _shopInteractionRegistry.Register(this);
        }

        protected override void OnDeactivate()
        {
            _shopInteractionRegistry?.Unregister(this);
        }
    }
}
