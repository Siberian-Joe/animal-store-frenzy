using System;

namespace Interfaces.Core
{
    public interface IViewModel : IDisposable
    {
        void Initialize();
        void Update();
        void FixedUpdate();
    }
}