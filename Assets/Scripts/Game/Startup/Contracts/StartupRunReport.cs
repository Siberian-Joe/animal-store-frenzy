using System.Collections.Generic;
using System.Linq;

namespace Game.Startup.Contracts
{
    public sealed class StartupRunReport
    {
        public IReadOnlyList<StartupTaskExecutionResult> Results { get; }

        public bool HasFailures => Results.Any(x => x.Status == StartupTaskExecutionStatus.Failed);

        public bool HasCriticalFailure => Results.Any(result =>
            result is { Status: StartupTaskExecutionStatus.Failed, FailurePolicy: StartupFailurePolicy.Critical });

        public StartupRunReport(IReadOnlyList<StartupTaskExecutionResult> results) => Results = results;
    }
}