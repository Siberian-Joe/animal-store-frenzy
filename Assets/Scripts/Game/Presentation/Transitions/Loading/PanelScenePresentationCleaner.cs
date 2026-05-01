using System;
using Modules.Presentation.Contracts.Navigation;
using Modules.SceneNavigation.Runtime.Contracts;

namespace Game.Presentation.Transitions.Loading
{
    public sealed class PanelScenePresentationCleaner : IScenePresentationCleaner
    {
        private readonly IPanelNavigator _panelNavigator;

        public PanelScenePresentationCleaner(IPanelNavigator panelNavigator)
        {
            _panelNavigator = panelNavigator ?? throw new ArgumentNullException(nameof(panelNavigator));
        }

        public void ClearScenePresentation()
        {
            _panelNavigator.Clear();
        }
    }
}