using System;

namespace Game.Presentation.Runtime.Preparation
{
    public interface IPanelPreparationScopePublisher
    {
        IDisposable Replace(IPanelPreparationScope scope);
    }
}