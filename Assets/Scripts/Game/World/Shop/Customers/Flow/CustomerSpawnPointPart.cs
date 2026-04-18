using System;
using Game.World.EntityRuntime;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Customers.Flow
{
    [DisallowMultipleComponent]
    public sealed class CustomerSpawnPointPart : EntityComponent
    {
        private ICustomerSpawnPointRegistry _spawnPointRegistry;

        public override int ActivationOrder => 150;

        [Inject]
        public void Construct(ICustomerSpawnPointRegistry spawnPointRegistry)
        {
            _spawnPointRegistry = spawnPointRegistry;
        }

        protected override void OnActivate()
        {
            if (_spawnPointRegistry == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomerSpawnPointPart)} on '{name}' requires {nameof(ICustomerSpawnPointRegistry)} injection.");
            }

            _spawnPointRegistry.Register(this);
        }

        protected override void OnDeactivate()
        {
            _spawnPointRegistry?.Unregister(this);
        }
    }
}