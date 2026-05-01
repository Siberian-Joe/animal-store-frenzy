using Modules.Startup.Contracts;

namespace Modules.Startup.Runtime
{
    public sealed class ApplicationStartup : PipelineStartup<IApplicationStartupTask>
    {
        public ApplicationStartup(StartupPipeline<IApplicationStartupTask> pipeline) : base(pipeline)
        {
        }
    }
}