using System;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Customers.Flow
{
    public sealed class CustomerFlowInstaller : MonoInstaller
    {
        [SerializeField] private CustomerFlowConfig _config;

        public override void InstallBindings()
        {
            if (_config == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomerFlowInstaller)} requires a {nameof(CustomerFlowConfig)} reference.");
            }

            _config.Validate();

            Container
                .BindInstance(_config)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<CustomerSpawnPointLocator>()
                .AsSingle();

            Container
                .BindInterfacesTo<CustomerFlowService>()
                .AsSingle();

            Container.BindExecutionOrder<CustomerFlowService>(100);
        }
    }
}
