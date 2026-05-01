using Modules.Presentation.Runtime.Preparation;

namespace Game.Presentation.Transitions.Loading
{
    public sealed class LoadingOverlayPresentationPlan : PanelPreparationPlan
    {
        public LoadingOverlayPresentationPlan()
        {
            Add<LoadingOverlayPresenter>();
        }
    }
}