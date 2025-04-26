using System.Collections.Generic;
using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface IOverlayRegistry : IPanelRegistry
    {
        IReadOnlyCollection<IPanelHandler> ActiveOverlays { get; }
    }
}