using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.Presentation.Configuration
{
    [CreateAssetMenu(
        fileName = "PresentationResourcesConfig",
        menuName = "Game/Presentation/Presentation Resources Config")]
    public sealed class PresentationResourcesConfig : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject _panelRootPrefab;
        [SerializeField] private AssetLabelReference _panelPrefabsLabel;

        public AssetReferenceGameObject PanelRootPrefab => _panelRootPrefab;
        public AssetLabelReference PanelPrefabsLabel => _panelPrefabsLabel;
    }
}