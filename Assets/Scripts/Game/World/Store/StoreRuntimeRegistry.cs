using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Store
{
    public sealed class StoreRuntimeRegistry : IStoreRuntimeRegistry, IStoreRuntimeResolver
    {
        private readonly Dictionary<EntityId, IStoreStatusWriter> _statusByStoreId = new();
        private readonly Dictionary<EntityId, ICustomerCycleProgressWriter> _progressByStoreId = new();

        public void Register(StoreStatusPart status)
        {
            if (status == false)
                throw new ArgumentNullException(nameof(status));

            var storeId = status.OwnerRoot.Id;
            if (_statusByStoreId.TryGetValue(storeId, out var existing) &&
                ReferenceEquals(existing, status) == false)
                throw new InvalidOperationException(
                    $"Duplicate store status for store '{storeId}' detected.");

            _statusByStoreId[storeId] = status;
        }

        public void Unregister(StoreStatusPart status)
        {
            if (status == false)
                return;

            var storeId = status.OwnerRoot.Id;
            if (_statusByStoreId.TryGetValue(storeId, out var existing) &&
                ReferenceEquals(existing, status)) _statusByStoreId.Remove(storeId);
        }

        public void Register(StoreCustomerCycleProgressPart progress)
        {
            if (progress == false)
                throw new ArgumentNullException(nameof(progress));

            var storeId = progress.OwnerRoot.Id;
            if (_progressByStoreId.TryGetValue(storeId, out var existing) &&
                ReferenceEquals(existing, progress) == false)
                throw new InvalidOperationException(
                    $"Duplicate store customer cycle progress for store '{storeId}' detected.");

            _progressByStoreId[storeId] = progress;
        }

        public void Unregister(StoreCustomerCycleProgressPart progress)
        {
            if (progress == false)
                return;

            var storeId = progress.OwnerRoot.Id;
            if (_progressByStoreId.TryGetValue(storeId, out var existing) &&
                ReferenceEquals(existing, progress)) _progressByStoreId.Remove(storeId);
        }

        public bool TryGetStatus(EntityId storeId, out IStoreStatus status)
        {
            if (_statusByStoreId.TryGetValue(storeId, out var writer))
            {
                status = writer;
                return true;
            }

            status = null;
            return false;
        }

        public bool TryGetStatusWriter(EntityId storeId, out IStoreStatusWriter status) =>
            _statusByStoreId.TryGetValue(storeId, out status);

        public bool TryGetOpenStore(out IStoreStatus status)
        {
            foreach (var candidate in _statusByStoreId.Values)
            {
                if (candidate == null || candidate.IsOpen == false)
                    continue;

                status = candidate;
                return true;
            }

            status = null;
            return false;
        }

        public bool TryGetAnyCycleProgressWriter(out ICustomerCycleProgressWriter progress)
        {
            foreach (var candidate in _progressByStoreId.Values)
            {
                if (candidate == null)
                    continue;

                progress = candidate;
                return true;
            }

            progress = null;
            return false;
        }
    }
}
