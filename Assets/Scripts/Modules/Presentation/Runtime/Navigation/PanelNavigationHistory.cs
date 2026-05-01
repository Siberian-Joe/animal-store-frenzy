using System;
using System.Collections.Generic;
using Modules.Presentation.Runtime.Contracts;
using Modules.Presentation.Runtime.Layers;

namespace Modules.Presentation.Runtime.Navigation
{
    public sealed class PanelNavigationHistory
    {
        private readonly List<IPanelHandle> _handles = new();

        public bool CanGoBack => _handles.Count > 1;

        public void Push(IPanelHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            if (_handles.Count > 0 && ReferenceEquals(_handles[^1], handle))
                return;

            _handles.Add(handle);
        }

        public bool TryGetCurrent(out IPanelHandle handle)
        {
            if (_handles.Count == 0)
            {
                handle = null;
                return false;
            }

            handle = _handles[^1];
            return true;
        }

        public bool TryPopCurrent(out IPanelHandle handle)
        {
            if (_handles.Count == 0)
            {
                handle = null;
                return false;
            }

            var index = _handles.Count - 1;
            handle = _handles[index];
            _handles.RemoveAt(index);

            return true;
        }

        public void CloseAndRemoveDescendantsOf(IPanelLayer layer)
        {
            if (layer == null)
                throw new ArgumentNullException(nameof(layer));

            for (var index = _handles.Count - 1; index >= 0; index--)
            {
                var handle = _handles[index];

                if (handle == null)
                {
                    _handles.RemoveAt(index);
                    continue;
                }

                if (handle.Layer.IsNavigationDescendantOf(layer) == false)
                    continue;

                if (handle.IsOpen)
                    handle.Close();

                _handles.RemoveAt(index);
            }
        }

        public void Remove(IPanelHandle handle)
        {
            if (handle == null)
                return;

            for (var index = _handles.Count - 1; index >= 0; index--)
            {
                if (ReferenceEquals(_handles[index], handle))
                    _handles.RemoveAt(index);
            }
        }

        public void Clear() => _handles.Clear();
    }
}