using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Panels;

namespace Game.Presentation.Transitions.Loading
{
    public sealed class LoadingOverlayPresenter : PanelPresenter<LoadingOverlay>
    {
        public void ShowImmediate() => Panel.ShowImmediate();

        public UniTask FadeInAsync(CancellationToken token) => Panel.FadeInAsync(token);

        public UniTask FadeOutAsync(CancellationToken token) => Panel.FadeOutAsync(token);
    }
}