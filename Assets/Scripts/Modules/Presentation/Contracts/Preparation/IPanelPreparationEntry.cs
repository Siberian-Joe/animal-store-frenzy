using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.Presentation.Contracts.Preparation
{
    public interface IPanelPreparationEntry
    {
        Type PresenterType { get; }

        UniTask PrepareAsync(IPanelPreparer preparer, CancellationToken token);
    }
}