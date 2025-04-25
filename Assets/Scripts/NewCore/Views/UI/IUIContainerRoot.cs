using UnityEngine;

namespace NewCore.Views.UI
{
    public interface IUIContainerRoot
    {
        Transform ScreensContainer { get; }
        Transform OverlayContainer { get; }
        Transform SystemOverlayContainer { get; }
        Transform PanelsCache { get; }
    }
}