using Game.World.Commands;
using Game.World.Shop.Commands;
using Game.World.UtilityAi;
using Zenject;

namespace Game.World.Shop
{
    public sealed class ShopCommandsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
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
        }
    }
}