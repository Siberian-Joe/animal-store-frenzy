using System;
using Game.Presentation.Contracts;

namespace Game.Presentation.Runtime.Preparation
{
    public interface IPanelLifetimeStore : IDisposable
    {
        void Add(IPanelLifetimeHandle handle);
    }
}