using System.Collections.Generic;

namespace Game.Presentation.Contracts.Preparation
{
    public interface IPanelPreparationPlan
    {
        IReadOnlyList<IPanelPreparationEntry> Entries { get; }
    }
}