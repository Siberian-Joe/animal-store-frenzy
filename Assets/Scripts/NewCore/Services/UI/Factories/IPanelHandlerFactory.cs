using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public interface IPanelHandlerFactory
    {
        UniTask<IPanelHandler<TViewModel>> CreateAsync<TPanel, TProxy, TViewModel>(UIContainerRoot roots,
            CancellationToken cancellationToken = default)
            where TPanel : PanelView<TViewModel>
            where TProxy : IProxy, new()
            where TViewModel : class, IViewModel;
    }
}