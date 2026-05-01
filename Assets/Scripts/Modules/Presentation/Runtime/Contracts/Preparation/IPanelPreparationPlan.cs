using System.Collections.Generic;

namespace Modules.Presentation.Runtime.Contracts.Preparation
{
    public interface IPanelPreparationPlan
    {
        IReadOnlyList<IPanelPreparationEntry> Entries { get; }
    }
}