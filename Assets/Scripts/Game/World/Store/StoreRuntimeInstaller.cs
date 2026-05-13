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

            if (container.HasBinding<OpenStoreCommandHandler>())
                return;

            container
                .Bind<OpenStoreCommandHandler>()
                .AsSingle();

            container
                .Bind<IGameCommandHandler>()
                .To<OpenStoreCommandHandler>()
                .FromResolve();
        }
    }
}