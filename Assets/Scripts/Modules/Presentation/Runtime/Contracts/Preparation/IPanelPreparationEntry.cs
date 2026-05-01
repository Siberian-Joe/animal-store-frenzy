using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.Presentation.Runtime.Contracts.Preparation
{
    public interface IPanelPreparationEntry
    {
        Type PresenterType { get; }

        UniTask PrepareAsync(IPanelPreparer preparer, CancellationToken token);
    }
}