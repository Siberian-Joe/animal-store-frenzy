using System;
using System.Collections.Generic;
using Modules.Presentation.Contracts;
using Modules.Presentation.Runtime.Handles;

namespace Modules.Presentation.Runtime.Layers
{
    public abstract class StackPanelLayer<TPanel> : PanelLayer<TPanel>
        where TPanel : class, IPanel
    {
        private readonly List<IPanelRuntimeHandle> _opened = new();

        public override void Open(IPanelRuntimeHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            handle.OpenIn(ContentRoot);

            if (_opened.Contains(handle) == false)
                _opened.Add(handle);
        }

        public override void Close(IPanelRuntimeHandle handle)
        {
            if (handle == null || handle.IsReleased)
                return;

            handle.CloseTo(CacheRoot);
            _opened.Remove(handle);
        }

        public override void Release(IPanelRuntimeHandle handle)
        {
            Close(handle);
            _opened.Remove(handle);
        }
    }
}