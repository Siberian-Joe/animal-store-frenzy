using System;

namespace Modules.Presentation.Runtime.Preparation
{
    public sealed class PanelPreparationScopeProvider :
        IPanelPreparationScopeAccess,
        IPanelPreparationScopePublisher
    {
        public IPanelPreparationScope Current { get; private set; }

        public PanelPreparationScopeProvider(IPanelPreparationScope projectScope) =>
            Current = projectScope ?? throw new ArgumentNullException(nameof(projectScope));

        public IDisposable Replace(IPanelPreparationScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var previous = Current;
            Current = scope;

            return new ScopeRollback(this, expectedCurrent: scope, previous: previous);
        }

        private void Rollback(IPanelPreparationScope expectedCurrent, IPanelPreparationScope previous)
        {
            if (ReferenceEquals(Current, expectedCurrent))
                Current = previous;
        }

        private sealed class ScopeRollback : IDisposable
        {
            private readonly IPanelPreparationScope _expectedCurrent;
            private readonly IPanelPreparationScope _previous;

            private PanelPreparationScopeProvider _owner;

            public ScopeRollback(
                PanelPreparationScopeProvider owner,
                IPanelPreparationScope expectedCurrent,
                IPanelPreparationScope previous)
            {
                _owner = owner ?? throw new ArgumentNullException(nameof(owner));
                _expectedCurrent = expectedCurrent ?? throw new ArgumentNullException(nameof(expectedCurrent));
                _previous = previous ?? throw new ArgumentNullException(nameof(previous));
            }

            public void Dispose()
            {
                if (_owner == null)
                    return;

                _owner.Rollback(_expectedCurrent, _previous);
                _owner = null;
            }
        }
    }
}