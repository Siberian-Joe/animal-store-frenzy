using System;

namespace Modules.Presentation.Runtime.Preparation
{
    public sealed class PanelPreparationScope : IPanelPreparationScope
    {
        public IPanelInstanceFactory InstanceFactory { get; }
        public IPanelLifetimeStore LifetimeStore { get; }

        public PanelPreparationScope(
            IPanelInstanceFactory instanceFactory,
            IPanelLifetimeStore lifetimeStore)
        {
            InstanceFactory = instanceFactory ?? throw new ArgumentNullException(nameof(instanceFactory));
            LifetimeStore = lifetimeStore ?? throw new ArgumentNullException(nameof(lifetimeStore));
        }
    }
}