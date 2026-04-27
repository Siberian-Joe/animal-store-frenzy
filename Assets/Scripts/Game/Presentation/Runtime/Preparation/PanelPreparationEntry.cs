using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Presentation.Contracts.Preparation;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Preparation
{
    public sealed class PanelPreparationEntry<TPresenter> : IPanelPreparationEntry
        where TPresenter : PanelPresenter
    {
        public Type PresenterType => typeof(TPresenter);

        public UniTask PrepareAsync(IPanelPreparer preparer, CancellationToken token) =>
            preparer?.PrepareAsync<TPresenter>(token) ?? throw new ArgumentNullException(nameof(preparer));
    }
}