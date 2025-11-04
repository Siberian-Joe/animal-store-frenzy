using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public interface IPanelHandlerFactory
    {
        UniTask<IPanelHandler<TViewModel>> CreateAsync<TPanel, TModel, TProxy, TViewModel>(UIContainerRoot roots,
            CancellationToken cancellationToken = default)
            where TPanel : PanelView<TViewModel>
            where TModel : IModel, new()
            where TProxy : IProxy
            where TViewModel : class, IViewModel;
    }
}