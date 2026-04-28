using UnityEngine;

namespace Modules.Presentation.Runtime.Handles
{
    public interface IPanelRuntimeHandle
    {
        bool IsOpen { get; }

        bool IsReleased { get; }

        void OpenIn(Transform parent);

        void CloseTo(Transform parent);
    }
}