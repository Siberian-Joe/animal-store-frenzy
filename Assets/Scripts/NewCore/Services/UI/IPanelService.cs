using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI
{
    public interface IPanelService
    {
        UniTask<IPanelHandler<TViewModel>> LoadPanelAsync<TPanel, TModel, TProxy, TViewModel>(
            CancellationToken cancellationToken = default)
            where TPanel : PanelView<TViewModel>
            where TModel : IModel, new()
            where TProxy : IProxy
            where TViewModel : class, IViewModel;
    }
}