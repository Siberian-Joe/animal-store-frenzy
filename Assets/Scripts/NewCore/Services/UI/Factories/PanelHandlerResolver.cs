using System;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;
using Zenject;

namespace NewCore.Services.UI.Factories
{
    public abstract class PanelHandlerResolver : IPanelHandlerResolver
    {
        protected readonly DiContainer Container;

        private readonly Type _markerInterface;
        private readonly Type _handlerGenericType;

        protected PanelHandlerResolver(DiContainer container, Type markerInterface, Type handlerGenericType)
        {
            Container = container;
            _markerInterface = markerInterface;
            _handlerGenericType = handlerGenericType;
        }

        public bool CanResolve(Type panelType)
            => _markerInterface.IsAssignableFrom(panelType);

        public IPanelHandler Resolve(IPanel panel, IViewModel viewModel, IUIContainerRoot uiRoots)
        {
            var closedHandlerType = _handlerGenericType.MakeGenericType(panel.GetType(), viewModel.GetType());

            return (IPanelHandler)Container.Instantiate(closedHandlerType, new object[] { panel, viewModel, uiRoots });
        }
    }
}