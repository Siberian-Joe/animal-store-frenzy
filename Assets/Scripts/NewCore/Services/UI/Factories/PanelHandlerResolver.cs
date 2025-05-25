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

        public bool CanResolve(Type panelType) => _markerInterface.IsAssignableFrom(panelType);

        public virtual IPanelHandler<TViewModel> Resolve<TPanel, TViewModel>(TPanel panel, TViewModel viewModel,
            IUIContainerRoot uiRoots) where TPanel : PanelView<TViewModel> where TViewModel : class, IViewModel
        {
            var handlerType = _handlerGenericType.MakeGenericType(typeof(TPanel), typeof(TViewModel));

            return (IPanelHandler<TViewModel>)Container.Instantiate(handlerType,
                new object[] { panel, viewModel, uiRoots });
        }
    }
}