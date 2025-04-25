using System.Collections.Generic;
using System.Linq;
using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public class OverlayRegistry : IOverlayRegistry
    {
        private readonly Stack<IPanelHandler> _stack = new();

        public IReadOnlyCollection<IPanelHandler> ActiveOverlays => _stack.Reverse().ToList();

        public void RegisterOverlay(IPanelHandler handler)
        {
            if (_stack.Count == 0 || _stack.Peek() != handler)
                _stack.Push(handler);
        }

        public void UnregisterOverlay(IPanelHandler handler)
        {
            if (_stack.Count > 0 && _stack.Peek() == handler)
                _stack.Pop();
        }
    }
}