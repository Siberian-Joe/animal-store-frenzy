using System;
using System.Collections.Generic;
using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public class PanelCache : IPanelCache
    {
        private readonly Dictionary<Type, IPanelHandler> _handlers = new();

        public bool TryGetHandler(Type panelType, out IPanelHandler handler)
            => _handlers.TryGetValue(panelType, out handler!);

        public void StoreHandler(Type panelType, IPanelHandler handler) => _handlers[panelType] = handler;

        public void RemoveHandler(Type panelType)
        {
            if (_handlers.ContainsKey(panelType))
                _handlers.Remove(panelType);
        }

        public IReadOnlyDictionary<Type, IPanelHandler> Handlers => _handlers;
    }
}