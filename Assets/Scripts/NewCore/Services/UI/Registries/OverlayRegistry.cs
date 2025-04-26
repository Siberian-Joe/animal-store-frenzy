using System.Collections.Generic;
using System.Linq;
using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public class OverlayRegistry : IOverlayRegistry
    {
        public IReadOnlyCollection<IPanelHandler> ActiveOverlays => _stack.Reverse().ToList();

        private readonly Stack<IPanelHandler> _stack = new();

        public void Register(IPanelHandler handler)
        {
            if (_stack.Count == 0 || _stack.Peek() != handler)
                _stack.Push(handler);
        }

        public void Unregister(IPanelHandler handler)
        {
            if (_stack.Count > 0 && _stack.Peek() == handler)
                _stack.Pop();
        }
    }
}