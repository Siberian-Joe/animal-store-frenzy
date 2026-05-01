using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Contracts.Preparation;
using Modules.Startup.Contracts;

namespace Game.Presentation.Startup
{
    [Startup(StartupPhase.Preparation, Order = 0)]
    public sealed class PrepareConfiguredPanelsStartupTask : ISceneStartupTask
    {
        private readonly IPanelPreparer _preparer;
        private readonly IReadOnlyList<IPanelPreparationPlan> _plans;

        public string Name => "Prepare configured panels";

        public PrepareConfiguredPanelsStartupTask(
            IPanelPreparer preparer,
            List<IPanelPreparationPlan> plans)
        {
            _preparer = preparer ?? throw new ArgumentNullException(nameof(preparer));
            _plans = plans ?? throw new ArgumentNullException(nameof(plans));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            foreach (var plan in _plans)
            {
                if (plan == null)
                    continue;

                foreach (var entry in plan.Entries)
                {
                    token.ThrowIfCancellationRequested();

                    if (entry == null)
                        continue;

                    await entry.PrepareAsync(_preparer, token);
                }
            }
        }
    }
}