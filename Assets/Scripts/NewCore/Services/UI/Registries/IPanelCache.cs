using System;
using System.Collections.Generic;
using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface IPanelCache
    {
        IReadOnlyDictionary<Type, IPanelHandler> Handlers { get; }

        bool TryGetHandler(Type panelType, out IPanelHandler handler);
        void StoreHandler(Type panelType, IPanelHandler handler);
    }
}