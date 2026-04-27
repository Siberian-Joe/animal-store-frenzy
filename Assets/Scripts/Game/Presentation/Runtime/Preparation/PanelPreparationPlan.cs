using System.Collections.Generic;
using Game.Presentation.Contracts.Preparation;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Preparation
{
    public abstract class PanelPreparationPlan : IPanelPreparationPlan
    {
        private readonly List<IPanelPreparationEntry> _entries = new();

        public IReadOnlyList<IPanelPreparationEntry> Entries => _entries;

        protected void Add<TPresenter>() where TPresenter : PanelPresenter =>
            _entries.Add(new PanelPreparationEntry<TPresenter>());
    }
}