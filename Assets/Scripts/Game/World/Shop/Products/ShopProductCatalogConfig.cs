using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.World.Shop.Products
{
    [CreateAssetMenu(
        fileName = "ShopProductCatalogConfig",
        menuName = "Game/Shop/Product Catalog Config")]
    public sealed class ShopProductCatalogConfig : ScriptableObject, IShopProductCatalogConfig
    {
        [SerializeField] private AssetReference[] _productReferences;

        public IReadOnlyList<AssetReference> ProductReferences =>
            _productReferences ?? Array.Empty<AssetReference>();
    }
}