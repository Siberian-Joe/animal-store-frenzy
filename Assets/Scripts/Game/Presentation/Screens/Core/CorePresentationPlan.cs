using Modules.Presentation.Runtime.Preparation;

namespace Game.Presentation.Screens.Core
{
    public sealed class CorePresentationPlan : PanelPreparationPlan
    {
        public CorePresentationPlan() => Add<CoreScreenPresenter>();
    }
}