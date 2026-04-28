using System;

namespace Modules.Presentation.Runtime.Preparation
{
    public interface IPanelPreparationScopePublisher
    {
        IDisposable Replace(IPanelPreparationScope scope);
    }
}