using System.Collections.Generic;

namespace Game.World.Shop.Products
{
    public interface IShopProductCatalogInitializer
    {
        void Initialize(IReadOnlyList<ProductDefinition> products);
    }
}