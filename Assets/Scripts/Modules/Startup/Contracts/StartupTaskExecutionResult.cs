using System;

namespace Modules.Startup.Contracts
{
    public readonly struct StartupTaskExecutionResult
    {
        public string Name { get; }
        public Type TaskType { get; }
        public StartupPhase Phase { get; }
        public int Order { get; }
        public StartupFailurePolicy FailurePolicy { get; }
        public StartupTaskExecutionStatus Status { get; }
        public TimeSpan Duration { get; }
        public Exception Exception { get; }

        public StartupTaskExecutionResult(
            string name,
            Type taskType,
            StartupPhase phase,
            int order,
            StartupFailurePolicy failurePolicy,
            StartupTaskExecutionStatus status,
            TimeSpan duration,
            Exception exception = null)
        {
            Name = name;
            TaskType = taskType;
            Phase = phase;
            Order = order;
            FailurePolicy = failurePolicy;
            Status = status;
            Duration = duration;
            Exception = exception;
        }
    }
}