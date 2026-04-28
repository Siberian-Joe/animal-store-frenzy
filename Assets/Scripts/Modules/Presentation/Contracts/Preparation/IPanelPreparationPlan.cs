using System.Collections.Generic;

namespace Modules.Presentation.Contracts.Preparation
{
    public interface IPanelPreparationPlan
    {
        IReadOnlyList<IPanelPreparationEntry> Entries { get; }
    }
}