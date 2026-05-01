using System;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Contracts
{
    public interface IPanelLifetimeHandle : IPanelHandle
    {
        Type PresenterType { get; }

        bool IsReleased { get; }

        void Release();
    }

    public interface IPanelLifetimeHandle<out TPresenter> :
        IPanelHandle<TPresenter>,
        IPanelLifetimeHandle
        where TPresenter : PanelPresenter
    {
    }
}