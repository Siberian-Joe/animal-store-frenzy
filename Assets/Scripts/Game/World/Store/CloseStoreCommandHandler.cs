using System;
using Game.World.Commands;
using Game.World.Persistence;

namespace Game.World.Store
{
    public sealed class CloseStoreCommandHandler : GameCommandHandler<CloseStoreCommand>
    {
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly IStoreRuntimeResolver _storeResolver;

        public CloseStoreCommandHandler(
            ILiveEntityRegistry liveEntityRegistry,
            IStoreRuntimeResolver storeResolver)
        {
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

        public override void Execute(CloseStoreCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntityRegistry.TryGet(command.StoreId, out var storeRoot) == false || storeRoot == false)
                throw new InvalidOperationException(
                    $"Cannot execute {nameof(CloseStoreCommand)} because store '{command.StoreId}' is not live.");

            if (_storeResolver.TryGetStatusWriter(command.StoreId, out var storeStatus) == false)
                throw new InvalidOperationException(
                    $"Store '{command.StoreId}' has no mutable {nameof(IStoreStatusWriter)}.");

            var storeShift = storeRoot.FindOwnedComponent<IStoreShiftWriter>();
            if (storeShift == null && _storeResolver.TryGetShiftWriter(command.StoreId, out storeShift) == false)
                throw new InvalidOperationException(
                    $"Store '{command.StoreId}' has no mutable {nameof(IStoreShiftWriter)}.");

            if (storeShift.CanCloseStore == false)
                throw new InvalidOperationException($"Store '{command.StoreId}' shift is not ready to close.");

            storeShift.CompleteShift();
            storeStatus.Close();
        }
    }
}