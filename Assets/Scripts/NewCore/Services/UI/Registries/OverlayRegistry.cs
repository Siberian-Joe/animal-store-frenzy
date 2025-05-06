using System.Collections.Generic;
using NewCore.Services.UI.Handlers;
using UnityEngine;

namespace NewCore.Services.UI.Registries
{
    public class OverlayRegistry : IOverlayRegistry
    {
        private readonly List<IPanelHandler> _handlers = new();

        public IReadOnlyCollection<IPanelHandler> ActiveOverlays => _handlers;
        public IPanelHandler TopOverlay => _handlers.Count > 0 ? _handlers[^1] : null;

        public void Register(IPanelHandler handler)
        {
            if (handler == null)
            {
                Debug.LogWarning("Trying to register null handler");
                return;
            }

            if (!_handlers.Contains(handler))
                _handlers.Add(handler);
        }

        public void Unregister(IPanelHandler handler)
        {
            if (handler == null)
            {
                Debug.LogWarning("Trying to unregister null handler");
                return;
            }

            _handlers.Remove(handler);
        }
    }
}