using System;

namespace Modules.Startup.Runtime.Contracts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class StartupAttribute : Attribute
    {
        public StartupPhase Phase { get; }

        public int Order { get; set; } = 0;

        public StartupFailurePolicy FailurePolicy { get; set; } = StartupFailurePolicy.Critical;

        public StartupExecutionMode ExecutionMode { get; set; } = StartupExecutionMode.Sequential;

        public StartupAttribute(StartupPhase phase) => Phase = phase;
    }
}