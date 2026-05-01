using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Contracts.Preparation;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Preparation
{
    public sealed class PanelPreparationEntry<TPresenter> : IPanelPreparationEntry
        where TPresenter : PanelPresenter
    {
        public Type PresenterType => typeof(TPresenter);

        public UniTask PrepareAsync(IPanelPreparer preparer, CancellationToken token) =>
            preparer?.PrepareAsync<TPresenter>(token) ?? throw new ArgumentNullException(nameof(preparer));
    }
}