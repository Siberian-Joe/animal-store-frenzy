using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Game.World.Shop.Products
{
    public interface IShopProductCatalogConfig
    {
        IReadOnlyList<AssetReference> ProductReferences { get; }
    }
}