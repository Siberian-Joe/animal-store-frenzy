using System.Collections.Generic;

namespace Game.World.Shop.Products
{
    public interface IShopProductCatalogProvider
    {
        IReadOnlyList<ProductDefinition> All { get; }

        ProductDefinition Get(ProductId productId);
    }
}