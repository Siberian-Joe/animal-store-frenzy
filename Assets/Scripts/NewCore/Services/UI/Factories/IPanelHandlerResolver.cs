using System;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public interface IPanelHandlerResolver
    {
        bool CanResolve(Type panelType);

        IPanelHandler Resolve(IPanel panel, IViewModel viewModel, IUIContainerRoot uiRoots);
    }
}