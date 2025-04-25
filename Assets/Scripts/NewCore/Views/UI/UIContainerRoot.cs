using UnityEngine;
 
 namespace NewCore.Views.UI
 {
     public class UIContainerRoot : MonoBehaviour, IUIContainerRoot
     {
         [field: Header("Screens Layer"),
                 Tooltip("Parent transform under which all screen panels are displayed (one active at a time)"),
                 SerializeField]
         public Transform ScreensContainer { get; private set; }
 
         [field: Header("Overlay Layer"),
                 Tooltip("Parent transform under which stacking overlays are displayed"),
                 SerializeField]
         public Transform OverlayContainer { get; private set; }
 
         [field: Tooltip("Parent transform under which system overlays are displayed (always on top, one at a time)"),
                 SerializeField]
         public Transform SystemOverlayContainer { get; private set; }
 
         [field: Header("Panels Cache"),
                 Tooltip("Parent transform under which inactive panels are kept for reuse"),
                 SerializeField]
         public Transform PanelsCache { get; private set; }
     }
 }