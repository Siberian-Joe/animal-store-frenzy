namespace Game.Startup.Contracts
{
    public interface IStartupTaskDescriptorResolver
    {
        StartupTaskDescriptor Resolve(IStartupTask task);
    }
}