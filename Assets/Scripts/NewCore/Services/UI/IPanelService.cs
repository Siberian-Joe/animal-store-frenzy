using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI
{
    public interface IPanelService
    {
        UniTask<IPanelHandler<TViewModel>> LoadPanelAsync<TPanel, TProxy, TViewModel>(
            CancellationToken cancellationToken = default)
            where TPanel : PanelView<TViewModel>
            where TProxy : IProxy, new()
            where TViewModel : class, IViewModel;
    }
}