using UnityEngine;

namespace Game.World.Shop.Products
{
    [CreateAssetMenu(
        fileName = "ProductDefinition",
        menuName = "Game/Shop/Product Definition")]
    public sealed class ProductDefinition : ScriptableObject
    {
        [SerializeField] private string _productId;
        [SerializeField] private string _displayName;

        public ProductId ProductId => new(_productId);

        public string DisplayName =>
            string.IsNullOrWhiteSpace(_displayName)
                ? _productId
                : _displayName;

        private void OnValidate()
        {
            _productId = string.IsNullOrWhiteSpace(_productId)
                ? string.Empty
                : _productId.Trim();

            _displayName = string.IsNullOrWhiteSpace(_displayName)
                ? string.Empty
                : _displayName.Trim();
        }
    }
}