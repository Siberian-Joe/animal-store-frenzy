using System;
using Modules.Presentation.Runtime.Contracts;

namespace Modules.Presentation.Runtime.Preparation
{
    public interface IPanelLifetimeStore : IDisposable
    {
        void Add(IPanelLifetimeHandle handle);
    }
}