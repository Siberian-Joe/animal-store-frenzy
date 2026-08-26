using Game.World.Commands;
using Game.World.Shop.Commands;
using Game.World.Shop.Customers;
using Game.World.Store;
using Game.World.UtilityAi;
using Zenject;

namespace Game.World.Shop
{
    public sealed class ShopRuntimeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            StoreRuntimeInstaller.InstallStoreRuntimeBindings(Container);

            Container
                .BindInterfacesAndSelfTo<ShopUtilityOpportunityLocator>()
                .AsSingle();

            Container
                .Bind<IUtilityReachabilityEvaluator>()
                .To<NavMeshUtilityReachabilityEvaluator>()
                .AsSingle();

            Container
                .Bind<ICustomerNeedResolution>()
                .To<CustomerNeedResolution>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<TakeProductFromShelfCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<WaitForCheckoutCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<CheckoutCustomerCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<LeaveStoreCommandHandler>()
                .AsSingle();
        }
    }
}