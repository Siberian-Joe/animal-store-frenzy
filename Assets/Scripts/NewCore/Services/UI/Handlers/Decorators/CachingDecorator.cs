using System;
using NewCore.Services.UI.Registries;
using NewCore.ViewModels;

namespace NewCore.Services.UI.Handlers.Decorators
{
    public class CachingDecorator<TViewModel> : PanelHandlerDecorator<TViewModel>
        where TViewModel : IViewModel
    {
        private readonly IPanelCache _cache;
        private readonly Type _panelType;

        public CachingDecorator(IPanelHandler<TViewModel> inner, IPanelCache cache, Type panelType) : base(inner)
        {
            _cache = cache;
            _panelType = panelType;

            _cache.StoreHandler(panelType, this);
        }

        public override void Dispose()
        {
            _cache.RemoveHandler(_panelType);
            base.Dispose();
        }
    }
}