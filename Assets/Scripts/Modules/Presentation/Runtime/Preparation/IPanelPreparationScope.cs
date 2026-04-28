namespace Modules.Presentation.Runtime.Preparation
{
    public interface IPanelPreparationScope
    {
        IPanelInstanceFactory InstanceFactory { get; }
        IPanelLifetimeStore LifetimeStore { get; }
    }
}