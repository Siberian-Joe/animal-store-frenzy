using System;
using Game.World.Commands;
using Game.World.Persistence;

namespace Game.World.Store
{
    public sealed class OpenStoreCommandHandler : GameCommandHandler<OpenStoreCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly IStoreRuntimeResolver _storeResolver;

        public OpenStoreCommandHandler(
            ILiveEntityRegistry liveEntityRegistry,
            IStoreRuntimeResolver storeResolver)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

        public override void Execute(OpenStoreCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.StoreId, out var storeRoot) == false || storeRoot == false)
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(OpenStoreCommand)} because store '{command.StoreId}' is not live.");

            if (_storeResolver.TryGetStatusWriter(command.StoreId, out var storeStatus) == false)
                throw new InvalidOperationException(
                    $"Store '{command.StoreId}' has no mutable {nameof(IStoreStatusWriter)}.");

            if (storeStatus.IsOpen)
                return;

            storeStatus.Open();

            if (storeRoot.TryFindOwnedComponent<ICustomerCycleProgressWriter>(out var localProgress))
                localProgress.Mark(CustomerCycleStage.StoreOpened);
            else if (_storeResolver.TryGetAnyCycleProgressWriter(out var progress))
                progress.Mark(CustomerCycleStage.StoreOpened);
        }
    }
}
