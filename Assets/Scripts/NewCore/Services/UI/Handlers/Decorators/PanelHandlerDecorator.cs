using System;
using NewCore.ViewModels;

namespace NewCore.Services.UI.Handlers.Decorators
{
    public abstract class PanelHandlerDecorator<TViewModel> : IPanelHandler<TViewModel>
        where TViewModel : IViewModel
    {
        protected readonly IPanelHandler<TViewModel> Inner;

        protected PanelHandlerDecorator(IPanelHandler<TViewModel> inner) =>
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));

        public virtual TViewModel Context => Inner.Context;

        public virtual void Open() => Inner.Open();

        public virtual void Close() => Inner.Close();

        public virtual void Dispose() => Inner.Dispose();
    }
}