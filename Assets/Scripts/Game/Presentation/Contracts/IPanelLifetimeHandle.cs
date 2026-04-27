using System;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Contracts
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