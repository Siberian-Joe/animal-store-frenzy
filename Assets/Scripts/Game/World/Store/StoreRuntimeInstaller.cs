using Game.World.Commands;
using Zenject;

namespace Game.World.Store
{
    public sealed class StoreRuntimeInstaller : MonoInstaller
    {
        public override void InstallBindings() => InstallStoreRuntimeBindings(Container);

        public static void InstallStoreRuntimeBindings(DiContainer container)
        {
            if (container.HasBinding<IStoreRuntimeResolver>() == false)
                container
                    .BindInterfacesAndSelfTo<StoreRuntimeRegistry>()
                    .AsSingle();

            BindCommandHandler<OpenStoreCommandHandler>(container);
            BindCommandHandler<CloseStoreCommandHandler>(container);
        }

        private static void BindCommandHandler<THandler>(DiContainer container)
            where THandler : IGameCommandHandler
        {
            if (container.HasBinding<THandler>())
                return;

            container
                .Bind<THandler>()
                .AsSingle();

            container
                .Bind<IGameCommandHandler>()
                .To<THandler>()
                .FromResolve();
        }
    }
}