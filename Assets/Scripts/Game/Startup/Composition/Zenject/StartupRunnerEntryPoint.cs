using System;
using Modules.Startup.Contracts;
using Modules.Startup.Runtime;
using Zenject;

namespace Game.Startup.Composition.Zenject
{
    public sealed class StartupRunnerEntryPoint<TStartup> : IInitializable
        where TStartup : class, IStartup
    {
        private readonly StartupRunner<TStartup> _runner;

        public StartupRunnerEntryPoint(StartupRunner<TStartup> runner) =>
            _runner = runner ?? throw new ArgumentNullException(nameof(runner));

        public void Initialize() => _runner.Initialize();
    }
}