using System;

namespace Game.Startup.Contracts
{
    public readonly struct StartupTaskDescriptor
    {
        public IStartupTask Task { get; }
        public Type TaskType { get; }
        public string Name { get; }
        public StartupPhase Phase { get; }
        public int Order { get; }
        public StartupFailurePolicy FailurePolicy { get; }
        public StartupExecutionMode ExecutionMode { get; }

        public StartupTaskDescriptor(
            IStartupTask task,
            Type taskType,
            string name,
            StartupPhase phase,
            int order,
            StartupFailurePolicy failurePolicy,
            StartupExecutionMode executionMode)
        {
            Task = task;
            TaskType = taskType;
            Name = name;
            Phase = phase;
            Order = order;
            FailurePolicy = failurePolicy;
            ExecutionMode = executionMode;
        }
    }
}