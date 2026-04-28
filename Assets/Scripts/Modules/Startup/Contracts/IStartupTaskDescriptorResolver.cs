namespace Modules.Startup.Contracts
{
    public interface IStartupTaskDescriptorResolver
    {
        StartupTaskDescriptor Resolve(IStartupTask task);
    }
}