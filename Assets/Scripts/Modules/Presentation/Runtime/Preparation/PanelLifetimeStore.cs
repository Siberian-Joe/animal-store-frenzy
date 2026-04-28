using System;
using System.Collections.Generic;
using Modules.Presentation.Contracts;

namespace Modules.Presentation.Runtime.Preparation
{
    public sealed class PanelLifetimeStore : IPanelLifetimeStore
    {
        private readonly List<IPanelLifetimeHandle> _handles = new();

        private bool _disposed;

        public void Add(IPanelLifetimeHandle handle)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(PanelLifetimeStore));

            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            if (_handles.Contains(handle))
                return;

            _handles.Add(handle);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            for (var index = _handles.Count - 1; index >= 0; index--)
            {
                var handle = _handles[index];

                if (handle is { IsReleased: false })
                    handle.Release();
            }

            _handles.Clear();
        }
    }
}