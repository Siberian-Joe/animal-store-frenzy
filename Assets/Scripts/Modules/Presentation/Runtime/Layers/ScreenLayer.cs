using System;
using Modules.Presentation.Runtime.Contracts;
using Modules.Presentation.Runtime.Handles;

namespace Modules.Presentation.Runtime.Layers
{
    public sealed class ScreenLayer : PanelLayer<IScreen>
    {
        private IPanelRuntimeHandle _current;

        public override void Open(IPanelRuntimeHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            if (_current != null && ReferenceEquals(_current, handle) == false && _current.IsReleased == false)
                Close(_current);

            handle.OpenIn(ContentRoot);
            _current = handle;
        }

        public override void Close(IPanelRuntimeHandle handle)
        {
            if (handle == null || handle.IsReleased)
                return;

            handle.CloseTo(CacheRoot);

            if (ReferenceEquals(_current, handle))
                _current = null;
        }

        public override void Release(IPanelRuntimeHandle handle)
        {
            if (handle == null)
                return;

            if (ReferenceEquals(_current, handle))
                _current = null;
        }
    }
}