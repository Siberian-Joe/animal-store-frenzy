using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Contracts.Preparation
{
    public interface IPanelPreparer
    {
        UniTask PrepareAsync<TPresenter>(CancellationToken token = default) where TPresenter : PanelPresenter;
    }
}