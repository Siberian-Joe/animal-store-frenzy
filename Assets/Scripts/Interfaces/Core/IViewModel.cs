using System;

namespace Interfaces.Core
{
    public interface IViewModel : IDisposable
    {
        void Update();
        void FixedUpdate();
    }
}