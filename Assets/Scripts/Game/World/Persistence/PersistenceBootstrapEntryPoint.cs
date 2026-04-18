using System;
using Zenject;

namespace Game.World.Persistence
{
    public sealed class PersistenceBootstrapEntryPoint : IInitializable, IDisposable
    {
        private readonly WorldPersistenceBootstrap _bootstrap;

        public PersistenceBootstrapEntryPoint(WorldPersistenceBootstrap bootstrap) => _bootstrap = bootstrap ?? throw new ArgumentNullException(nameof(bootstrap));

        public void Initialize() => _bootstrap.Initialize();

        public void Dispose() => _bootstrap.Dispose();
    }
}