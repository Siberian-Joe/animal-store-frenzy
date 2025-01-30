using System;
using Zenject;

namespace NewCore.Bootstrap
{
    public interface IBootstrapper : IInitializable, IDisposable
    {
    }
}