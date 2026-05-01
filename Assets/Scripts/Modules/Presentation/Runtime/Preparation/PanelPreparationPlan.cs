using System.Collections.Generic;
using Modules.Presentation.Runtime.Contracts.Preparation;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Preparation
{
    public abstract class PanelPreparationPlan : IPanelPreparationPlan
    {
        private readonly List<IPanelPreparationEntry> _entries = new();

        public IReadOnlyList<IPanelPreparationEntry> Entries => _entries;

        protected void Add<TPresenter>() where TPresenter : PanelPresenter =>
            _entries.Add(new PanelPreparationEntry<TPresenter>());
    }
}