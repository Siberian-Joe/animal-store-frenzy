namespace Modules.Startup.Runtime.Contracts
{
    public interface IStartupTaskDescriptorResolver
    {
        StartupTaskDescriptor Resolve(IStartupTask task);
    }
}