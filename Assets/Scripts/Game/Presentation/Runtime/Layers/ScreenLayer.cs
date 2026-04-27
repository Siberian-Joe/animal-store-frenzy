using System;
using Game.Presentation.Contracts;
using Game.Presentation.Runtime.Handles;

namespace Game.Presentation.Runtime.Layers
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

        public override void Release(IPanelRuntimeHandle handle) => Close(handle);
    }
}