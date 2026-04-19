using System;
using Game.World.Commands;
using Game.World.Shop.Commands;
using Game.World.Shop.Products;
using Game.World.UtilityAi;
using UnityEngine;
using Zenject;

namespace Game.World.Shop
{
    public sealed class ShopRuntimeInstaller : MonoInstaller
    {
        [SerializeField] private ShopProductCatalogConfig _productCatalogConfig;

        public override void InstallBindings()
        {
            if (_productCatalogConfig == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(ShopRuntimeInstaller)} requires a {nameof(ShopProductCatalogConfig)} reference.");
            }

            Container
                .BindInterfacesAndSelfTo<ShopUtilityOpportunityLocator>()
                .AsSingle();

            Container
                .Bind<IUtilityReachabilityEvaluator>()
                .To<NavMeshUtilityReachabilityEvaluator>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<TakeProductFromShelfCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<CheckoutCustomerCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<LeaveStoreCommandHandler>()
                .AsSingle();

            Container
                .Bind<IShopProductCatalogConfig>()
                .FromInstance(_productCatalogConfig)
                .AsSingle();

            Container
                .BindInterfacesTo<ShopProductCatalogProvider>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ShopProductCatalogStartupTask>()
                .AsSingle();
        }
    }
}