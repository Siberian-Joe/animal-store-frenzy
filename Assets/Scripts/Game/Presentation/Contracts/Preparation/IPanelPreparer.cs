using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Contracts.Preparation
{
    public interface IPanelPreparer
    {
        UniTask PrepareAsync<TPresenter>(CancellationToken token = default) where TPresenter : PanelPresenter;
    }
}